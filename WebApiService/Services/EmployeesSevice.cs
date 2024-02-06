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

        public async Task<List<EmployeeDto>> Get(string? search, int? page)
        {
            //login imie nazwisko w tabeli users, employee tylko id

            return await _context.Employees
                .Where(u =>
                    !string.IsNullOrEmpty(search) ? 
                    (
                        u.Login.ToLower().Contains(search.ToLower()) ||
                        string.Concat(u.Name.ToLower(), " ", u.LastName.ToLower()).Trim().Contains(search.ToLower())
                    ) : true                                            
                )
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select(u => new EmployeeDto
                {
                    Id = u.Id,
                    Login = u.Login,
                    Name = $"{u.Name} {u.LastName}".Trim(),
                    Type = u.Type,
                })
                .OrderBy(u => u.Id)
                .ToListAsync();            
        }                   
    }
}
