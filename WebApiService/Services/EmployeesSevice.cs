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
                            u.User.Name.ToLower().Contains(search.ToLower())
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
                    Type = u.Type,
                })
                .ToListAsync();

            //return await _context.Employees
            //    .Where(u =>
            //        u.IsActive &&
            //        (
            //            !string.IsNullOrEmpty(search) ? 
            //            (                        
            //                u.Login.ToLower().Contains(search.ToLower()) ||
            //                u.Name.ToLower().Contains(search.ToLower())                        
            //            ) : true
            //        ) 
            //        &&
            //        (
            //            type != null && type != 0 ? u.Type == type : true
            //        )
            //    )
            //    .OrderBy(sortColumn ?? nameof(EmployeeListDto.Id), isDescending)
            //    .Skip(50 * ((page ?? 1) - 1))
            //    .Take(50)
            //    .Select(u => new EmployeeListDto
            //    {
            //        Id = u.Id,
            //        Login = u.Login,
            //        Name = u.Name,
            //        Type = u.Type,
            //    })                
            //    .ToListAsync();            
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

        public async Task<Boolean> Delete(int id)
        {
            var model = await _context.Employees
               .Include(u => u.User)
               .SingleOrDefaultAsync(u => u.Id == id);

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

        private IQueryable<EmployeeModel> ApplySorting(IQueryable<EmployeeModel> query, string? sortColumn, string? sortOrder)
        {
            //var sql = query.ToQueryString();    

            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);
            sortColumn = sortColumn ?? nameof(EmployeeListDto.Id);

            if (sortColumn.Equals(nameof(EmployeeListDto.Login), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.User.Login, isDescending);

            if (sortColumn.Equals(nameof(EmployeeListDto.Name), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.User.Name, isDescending);

            return query.OrderBy(sortColumn, isDescending);
        }
    }
}
