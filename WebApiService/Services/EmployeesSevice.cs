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

        public async Task<List<EmployeeDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page)
        {
            //login imie nazwisko w tabeli users, employee tylko id
            //var sql = query.ToQueryString();
            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);

            return await _context.Employees
                .Where(u =>
                    (
                        !string.IsNullOrEmpty(search) ? 
                        (                        
                            u.Login.ToLower().Contains(search.ToLower()) ||
                            u.Name.ToLower().Contains(search.ToLower())                        
                        ) : true
                    ) 
                    &&
                    (
                        type != null && type != 0 ? u.Type == type : true
                    )
                )
                .OrderBy(sortColumn ?? nameof(EmployeeDto.Id), isDescending)
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select(u => new EmployeeDto
                {
                    Id = u.Id,
                    Login = u.Login,
                    Name = u.Name,
                    Type = u.Type,
                })                
                .ToListAsync();            
        }

        public async Task<Boolean> Delete(int id)
        {
            var item = await _context.Employees.FindAsync(id);
            if (item == null)
            {
                _logger.LogWarning($"Employee id: {id} not found");
                return false;
            }

            _context.Employees.Remove(item);

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
