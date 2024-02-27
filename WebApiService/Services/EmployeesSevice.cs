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
            //var sql = query.ToQueryString();

            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);

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

            query = query.OrderBy(u => u.User.Name, isDescending);

            return await query
                //.OrderBy(sortColumn ?? nameof(EmployeeListDto.Id), isDescending)
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

        public async Task<EmployeeModel?> Get(int id)
        {
            return await _context.Employees.FindAsync(id);                
        }

        public async Task<Boolean> Delete(int id)
        {
            var model = await _context.Employees.FindAsync(id);
            if (model == null)
            {
                _logger.LogWarning($"Employee id: {id} not found");
                return false;
            }

            //model.IsActive = false;
            //_context.Employees.Remove(model);

            var result = await _context.SaveChangesAsync();
            
            return result > 0;
        }

        public async Task<Boolean> DeleteAll()
        {
            var result = await _context.Database.ExecuteSqlRawAsync("delete from Employees");

            return result > 0;            
        }
    }
}
