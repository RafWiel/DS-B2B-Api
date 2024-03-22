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
    public class EmployeesService : IEmployeesService
    {
        private readonly DataContext _context;
        private readonly ILogger<IEmployeesService> _logger;

        public EmployeesService(DataContext context, ILogger<IEmployeesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<EmployeeListDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page)
        {                    
            var query = _context.Employees
                .Include(u => u.User)
                .Where(u =>
                    u.User.IsActive &&
                    (
                        !string.IsNullOrEmpty(search) ?
                        (
                            u.User.Login.ToLower().Contains(search.ToLower()) ||
                            u.User.Name.ToLower().Contains(search.ToLower()) ||
                            u.User.PhoneNumber.ToLower().Contains(search.ToLower())
                        ) : true
                    )
                    &&
                    (
                        type != null && type != 0 ? u.Type == type : true
                    )
                );

            query = ApplySorting(query, sortColumn, sortOrder); 

            return await query                
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select(u => new EmployeeListDto
                {
                    Id = u.Id,
                    Login = u.User.Login,
                    Name = u.User.Name,
                    PhoneNumber = u.User.PhoneNumber,
                    Type = u.Type,
                })
                .ToListAsync();                    
        }

        private IQueryable<EmployeeModel> ApplySorting(IQueryable<EmployeeModel> query, string? sortColumn, string? sortOrder)
        {
            //var sql = query.ToQueryString();    

            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);
            sortColumn = sortColumn ?? nameof(EmployeeListDto.Login);

            if (sortColumn.Equals(nameof(EmployeeListDto.Login), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.User.Login, isDescending);

            if (sortColumn.Equals(nameof(EmployeeListDto.Name), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.User.Name, isDescending);

            if (sortColumn.Equals(nameof(EmployeeListDto.PhoneNumber), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.User.PhoneNumber, isDescending);

            return query.OrderBy(sortColumn, isDescending);
        }

        public async Task<EmployeeDto?> GetSingle(int id)
        {
            var model = await _context.Employees
                .Include(u => u.User)
                .SingleOrDefaultAsync(u => u.Id == id);                

            return model == null ? null : new EmployeeDto
            {
                Id = model.Id,
                Type = model.Type,
                Login = model.User.Login,
                Name = model.User.Name,
                PhoneNumber = model.User.PhoneNumber,
                Email = model.User.Email,
                IsMailing = model.IsMailing
            };
        }

        public async Task<IdResponseModel> Add(EmployeeDto dto)
        {
            var model = await _context.Employees
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
                _logger.LogWarning($"Employee {dto.Login} already exists");

                return new IdResponseModel
                { 
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            model = new EmployeeModel
            {
                User = new UserModel
                {
                    Login = dto.Login,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    IsActive = true
                },
                Type = dto.Type,
                IsMailing = dto.IsMailing
            };

            _context.Employees.Add(model);

            var result = await _context.SaveChangesAsync();

            return new IdResponseModel
            {
                Id = result <= 0 ? 0 : model.Id,
                StatusCode = result <= 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK
            };
        }

        public async Task<IdResponseModel> Update(EmployeeDto dto)
        {
            var model = await _context.Employees
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
                _logger.LogWarning($"Employee {dto.Login} login, email, phone number conflict");

                return new IdResponseModel
                {
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            model = await _context.Employees
               .Include(u => u.User)
               .SingleOrDefaultAsync(u => u.Id == dto.Id);

            if (model == null)
            {
                _logger.LogWarning($"Employee id: {dto.Id} not found");

                return new IdResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }            

            model.User.Login = dto.Login;
            model.User.Name = dto.Name;
            model.User.PhoneNumber = dto.PhoneNumber;
            model.User.Email = dto.Email;
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
            var model = await _context.Employees
                .Include(u => u.User)
                .SingleOrDefaultAsync(u => u.Id == id && u.User.IsActive);

            if (model == null)
            {
                _logger.LogWarning($"Employee id: {id} not found");
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
                inner join Employees on Users.Id = Employees.UserId
            ");

            return result > 0;            
        }             
    }
}
