using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using WebApiService.Interfaces;
using WebApiService.Enums;
using WebApiService.Models;

namespace WebApiService.Services
{
    public class InstallService : IInstallService
    {
        private readonly DataContext _context;
        private readonly ILogger<IInstallService> _logger;

        public InstallService(DataContext context, ILogger<IInstallService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Boolean> Install()
        {
            InstallEmployees();
            
            await _context.SaveChangesAsync();

            return true;
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
