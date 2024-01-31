using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using WebApiService.Interfaces;
using WebApiService.Enums;
using WebApiService.Models;
using WebApiService.DataTransferObjects;

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

        public async Task<List<EmployeeDto>> Get()
        {
            //return all
            //if (string.IsNullOrEmpty(search))
            return await _context
                .Set<EmployeeModel>()
                .Select(u => new EmployeeDto
                {
                    Id = u.Id,                    
                    Login = u.Login,
                    Name = $"{u.Name} {u.LastName}".Trim(),
                    Type = u.Type,
                })
                .ToListAsync();
        }        

        #region Employees

        private void InstallEmployees()
        {
            if (_context.Employees.Count() > 0)
                return;

            _context.Employees.Add(new EmployeeModel
            {
                Type = (int)EmployeeType.Administrator,
                Login = "rafal.wielicki",
                Name = "Rafał",
                LastName = "Wielicki",
                PhoneNumber = "793008557",
                Email = "rafal.wielicki@gmail.com",
                IsActive = true,
                IsMailing = false
            });

            _context.Employees.Add(new EmployeeModel
            {
                Type = (int)EmployeeType.Administrator,
                Login = "piotr.trybuchowicz",
                Name = "Piotr",
                LastName = "Trybuchowicz",
                PhoneNumber = "793008558",
                Email = "piotr.trybuchowicz@gmail.com",
                IsActive = true,
                IsMailing = false
            });

            _context.Employees.Add(new EmployeeModel
            {
                Type = (int)EmployeeType.Supervisor,
                Login = "andrzej.jurkowski",
                Name = "Andrzej",
                LastName = "Jurkowski",
                PhoneNumber = "793008559",
                Email = "andrzej.jurkowski@gmail.com",
                IsActive = true,
                IsMailing = false
            });
        }

        #endregion        
    }
}
