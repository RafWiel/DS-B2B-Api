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
            InstallCompanies();
            InstallCustomers();            

            await _context.SaveChangesAsync();

            return true;
        }        

        #region Employees

        private void InstallEmployees()
        {
            if (_context.Employees.Count() > 0)
                return;            

            var random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                _context.Employees.Add(new EmployeeModel
                {
                    User = new UserModel
                    {
                        Login = $"employee_{i}",
                        Name = $"Name {i}",
                        PhoneNumber = $"123456{i:000}",
                        Email = $"employee_{i}@email.com",
                        IsActive = true,
                    },
                    Type = (int)random.Next(1, 3),                    
                    IsMailing = false
                });
            }
        }

        #endregion

        #region Customers

        private void InstallCustomers()
        {
            if (_context.Customers.Count() > 0)
                return;

            var random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                _context.Customers.Add(new CustomerModel
                {
                    User = new UserModel
                    {
                        Login = $"customer_{i}",
                        Name = $"Name {i}",
                        PhoneNumber = $"654321{i:000}",
                        Email = $"customer_{i}@email.com",
                        IsActive = true,
                    },
                    Type = (int)random.Next(2, 3),
                    IsMailing = false
                });
            }
        }

        #endregion

        #region Companies

        private void InstallCompanies()
        {
            if (_context.Companies.Count() > 0)
                return;

            var random = new Random();
            for (int i = 1; i <= 10; i++)
            {
                _context.Companies.Add(new CompanyModel
                {
                    ErpId = i,
                    Name = $"Company_{i}",
                    TaxNumber = $"12345678{i:00}",
                    Address = $"Address_{i}",
                    Postal = "00-000",
                    City = "City",
                    IsActive = true
                });
            }
        }

        #endregion  
    }
}
