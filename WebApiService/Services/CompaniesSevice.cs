using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using WebApiService.Interfaces;
using WebApiService.Enums;
using WebApiService.Models;
using WebApiService.DataTransferObjects;
using System.Text.RegularExpressions;
using WebApiService.Extensions;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace WebApiService.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly DataContext _context;
        private readonly ILogger<ICompaniesService> _logger;

        public CompaniesService(DataContext context, ILogger<ICompaniesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CompanyListDto>> Get(string? search, string? sortColumn, string? sortOrder, int? page)
        {
            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);
            sortColumn = sortColumn ?? nameof(EmployeeListDto.Id);

            return await _context.Companies
                .Where(u =>
                    u.IsActive &&
                    (
                        !string.IsNullOrEmpty(search) ?
                        (                            
                            u.Name.ToLower().Contains(search.ToLower()) ||
                            u.ErpId.ToString().ToLower().Contains(search.ToLower()) ||
                            u.TaxNumber.ToLower().Contains(search.ToLower()) ||
                            u.City.ToLower().Contains(search.ToLower())
                        ) : true
                    )                    
                )
                .OrderBy(sortColumn ?? nameof(CompanyModel.Id), isDescending)
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select(u => new CompanyListDto
                {
                    Id = u.Id,
                    ErpId = u.ErpId,
                    Name = u.Name,
                    TaxNumber = u.TaxNumber,
                    City = u.City
                })
                .ToListAsync();
        }

        public async Task<List<ListDto>> GetList()
        {
            var items = await _context.Companies
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .ToListAsync();

            //dodaj na poczatku pusta pozycje
            if (items.Count > 0)
                items.Insert(0, new CompanyModel());

            return items.Select(u => new ListDto
            {
                Id = u.Id,
                Name = u.Name
            })
            .ToList();
        }

        public async Task<CompanyDto?> GetSingle(int id)
        {
            var model = await _context.Companies  
                .Include(u => u.Customers
                    .Where(u => u.User.IsActive))                
                .ThenInclude(u => u.User)                
                .SingleOrDefaultAsync(u => u.Id == id);

            return model == null ? null : new CompanyDto
            {
                Id = model.Id,                
                Name = model.Name,
                ErpId = model.ErpId,
                TaxNumber = model.TaxNumber,
                Address = model.Address,
                Postal = model.Postal,
                City = model.City,
                Customers = model.Customers
                    .Select(u => new CustomerListDto
                    {
                        Id = u.Id,
                        Login = u.User.Login,
                        Name = u.User.Name,
                        PhoneNumber = u.User.PhoneNumber,
                        Type = u.Type
                    })
                    .ToList()
            };
        }

        public async Task<ResponseModel> Add(CompanyDto dto)
        {
            var model = await _context.Companies
               .FirstOrDefaultAsync(u =>
                    u.IsActive &&
                    (                        
                        u.Name.ToLower().Equals(dto.Name.ToLower()) ||
                        u.ErpId == dto.ErpId ||
                        u.TaxNumber.ToLower().Equals(dto.TaxNumber.ToLower())
                    )
                );

            if (model != null)
            {
                _logger.LogWarning($"Company {dto.Name} already exists");

                return new ResponseModel 
                { 
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            model = new CompanyModel
            {
                Name = dto.Name,
                ErpId = dto.ErpId,                
                TaxNumber = dto.TaxNumber,
                Address = dto.Address,
                Postal = dto.Postal,
                City = dto.City,
                IsActive = true
            };

            _context.Companies.Add(model);

            var result = await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Id = result <= 0 ? 0 : model.Id,
                StatusCode = result <= 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK
            };
        }

        public async Task<ResponseModel> Update(CompanyDto dto)
        {
            var model = await _context.Companies
                .FirstOrDefaultAsync(u =>
                    u.Id != dto.Id &&
                    u.IsActive &&
                    (                        
                        u.Name.ToLower().Equals(dto.Name.ToLower()) ||
                        u.ErpId == dto.ErpId ||
                        u.TaxNumber.ToLower().Equals(dto.TaxNumber.ToLower())
                    )
                );

            if (model != null)
            {
                _logger.LogWarning($"Company {dto.Name} ERP Id, name, tax number conflict");

                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            model = await _context.Companies
               .SingleOrDefaultAsync(u => u.Id == dto.Id);

            if (model == null)
            {
                _logger.LogWarning($"Company id: {dto.Id} not found");

                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            
            model.Name = dto.Name;
            model.ErpId = dto.ErpId;
            model.TaxNumber = dto.TaxNumber;
            model.Address = dto.Address;
            model.Postal = dto.Postal;
            model.City = dto.City;

            var result = await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Id = model.Id,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<Boolean> Delete(int id)
        {
            var model = await _context.Companies
               .SingleOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (model == null)
            {
                _logger.LogWarning($"Company id: {id} not found");
                return false;
            }

            model.IsActive = false;

            var result = await _context.SaveChangesAsync();

            //usun pracownikow
            await _context.Database.ExecuteSqlRawAsync(
                $"update Users " +
                $"set IsActive = 0 " +
                $"from Users " +
                $"inner join Customers on Users.Id = Customers.UserId " +
                $"where Customers.CompanyModelId = {model.Id}");

            return result > 0;
        }

        public async Task<Boolean> DeleteAll()
        {
            var result = await _context.Database.ExecuteSqlRawAsync(@"
                update Companies 
                set IsActive = 0                                 

                update Users 
                set IsActive = 0 
                from Users 
                inner join Customers on Users.Id = Customers.UserId
            ");        

            return result > 0;
        }
    }
}
