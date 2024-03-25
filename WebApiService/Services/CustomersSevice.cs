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
    public class CustomersService : ICustomersService
    {
        private readonly DataContext _context;
        private readonly ILogger<ICustomersService> _logger;

        private class ExtendedCustomerModel
        {
            public CustomerModel Customer { get; set; }
            public string CompanyName { get; set; }
        }

        public CustomersService(DataContext context, ILogger<ICustomersService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CustomerListDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page)
        {            
            var query = _context.Customers
                .Include(u => u.User)
                .Join(_context.Companies, 
                    customer => customer.CompanyModelId, 
                    company => company.Id,
                    (customer, company) => new ExtendedCustomerModel 
                    { 
                        Customer = customer, 
                        CompanyName = company.Name
                    })
                .Where(u =>
                    u.Customer.User.IsActive &&
                    (
                        !string.IsNullOrEmpty(search) ?
                        (
                            u.Customer.User.Login.ToLower().Contains(search.ToLower()) ||
                            u.Customer.User.Name.ToLower().Contains(search.ToLower()) ||
                            u.Customer.User.PhoneNumber.ToLower().Contains(search.ToLower()) ||
                            u.CompanyName.ToLower().Contains(search.ToLower())
                        ) : true
                    )
                    &&
                    (
                        type != null && type != 0 ? u.Customer.Type == type : true
                    )
                );

            query = ApplySorting(query, sortColumn, sortOrder); 

            return await query                
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select(u => new CustomerListDto
                {
                    Id = u.Customer.Id,
                    Login = u.Customer.User.Login,
                    Name = u.Customer.User.Name,
                    PhoneNumber = u.Customer.User.PhoneNumber,                    
                    Type = DataContext.GetCustomerType(u.Customer.Type),
                    CompanyName = u.CompanyName
                })
                .ToListAsync();                    
        }

        private IQueryable<ExtendedCustomerModel> ApplySorting(IQueryable<ExtendedCustomerModel> query, string? sortColumn, string? sortOrder)
        {
            //var sql = query.ToQueryString();    

            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);
            sortColumn = sortColumn ?? nameof(CustomerListDto.Login);

            if (sortColumn.Equals(nameof(CustomerListDto.Login), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Customer.User.Login, isDescending);

            if (sortColumn.Equals(nameof(CustomerListDto.Name), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Customer.User.Name, isDescending);

            if (sortColumn.Equals(nameof(CustomerListDto.PhoneNumber), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Customer.User.PhoneNumber, isDescending);

            if (sortColumn.Equals(nameof(CustomerListDto.Type), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetCustomerTypeSorting(u.Customer.Type), isDescending);

            if (sortColumn.Equals(nameof(CustomerListDto.CompanyName), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.CompanyName, isDescending);

            if (sortColumn.Equals(nameof(CustomerListDto.Type), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Customer.Type, isDescending);

            return query.OrderByWithDirection(u => u.Customer.User.Login, isDescending);
        }

        public async Task<CustomerDto?> GetSingle(int id)
        {
            var model = await _context.Customers
                .Include(u => u.User)
                .SingleOrDefaultAsync(u => u.Id == id);                

            return model == null ? null : new CustomerDto
            {
                Id = model.Id,
                CompanyId = model.CompanyModelId,
                Type = model.Type,
                Login = model.User.Login,
                Name = model.User.Name,
                PhoneNumber = model.User.PhoneNumber,
                Email = model.User.Email,
                IsMailing = model.IsMailing
            };
        }

        public async Task<IdResponseModel> Add(CustomerDto dto)
        {
            var model = await _context.Customers
               .FirstOrDefaultAsync(u =>
                    u.User.IsActive &&
                    (
                        u.User.Login.ToLower().Equals(dto.Login.ToLower()) ||
                        u.User.Email.ToLower().Equals(dto.Email.ToLower()) ||
                        u.User.PhoneNumber.ToLower().Equals(dto.PhoneNumber.ToLower())
                    )
                );

            if (model != null)
            {
                _logger.LogWarning($"Customer {dto.Login} already exists");

                return new IdResponseModel
                { 
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            model = new CustomerModel
            {
                User = new UserModel
                {                    
                    Login = dto.Login,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    IsActive = true
                },
                CompanyModelId = dto.CompanyId,
                Type = dto.Type,
                IsMailing = dto.IsMailing
            };

            _context.Customers.Add(model);

            var result = await _context.SaveChangesAsync();

            return new IdResponseModel
            {
                Id = result <= 0 ? 0 : model.Id,
                StatusCode = result <= 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK
            };
        }

        public async Task<IdResponseModel> Update(CustomerDto dto)
        {
            var model = await _context.Customers
                .FirstOrDefaultAsync(u =>
                    u.Id != dto.Id && 
                    u.User.IsActive &&
                    (
                        u.User.Login.ToLower().Equals(dto.Login.ToLower()) ||
                        u.User.Email.ToLower().Equals(dto.Email.ToLower()) ||
                        u.User.PhoneNumber.ToLower().Equals(dto.PhoneNumber.ToLower())
                    )
                );

            if (model != null)
            {
                _logger.LogWarning($"Customer {dto.Login} login, email, phone number conflict");
                
                return new IdResponseModel
                {
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            model = await _context.Customers
               .Include(u => u.User)
               .SingleOrDefaultAsync(u => u.Id == dto.Id);

            if (model == null)
            {
                _logger.LogWarning($"Customer id: {dto.Id} not found");

                return new IdResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }            

            model.User.Login = dto.Login;
            model.User.Name = dto.Name;
            model.User.PhoneNumber = dto.PhoneNumber;
            model.User.Email = dto.Email;
            model.CompanyModelId = dto.CompanyId;
            model.Type = dto.Type;
            model.IsMailing = dto.IsMailing;
                       
            var result = await _context.SaveChangesAsync();
            
            return new IdResponseModel
            {
                Id = model.Id,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<Boolean> Delete(int id)
        {
            var model = await _context.Customers
               .Include(u => u.User)
               .SingleOrDefaultAsync(u => u.Id == id && u.User.IsActive);

            if (model == null)
            {
                _logger.LogWarning($"Customer id: {id} not found");
                return false;
            }

            model.User.IsActive = false;

            var result = await _context.SaveChangesAsync();
            
            return result > 0;
        }

        public async Task<Boolean> DeleteAll()
        {
            var result = await _context.Database.ExecuteSqlRawAsync(@"
                update Users 
                set IsActive = 0 
                from Users 
                inner join Customers on Users.Id = Customers.UserId
            ");

            return result > 0;            
        }

        public async Task<Boolean> DeleteAllCompany(int companyId)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                $"update Users " +
                $"set IsActive = 0 " +
                $"from Users " +
                $"inner join Customers on Users.Id = Customers.UserId " +
                $"where Customers.CompanyModelId = {companyId}");

            return result > 0;
        }             
    }
}
