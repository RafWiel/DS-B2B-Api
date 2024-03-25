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
using WebApiService.Data;

namespace WebApiService.Services
{
    public class ServiceRequestsService : IServiceRequestsService
    {
        private readonly DataContext _context;
        private readonly ILogger<IServiceRequestsService> _logger;

        public ServiceRequestsService(DataContext context, ILogger<IServiceRequestsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ServiceRequestListDto>> Get(
            string? search, 
            int? type, 
            int? submitType, 
            int? status, 
            string? sortColumn, 
            string? sortOrder, 
            int? page)
        {
            var query = _context.ServiceRequests
                .Where(u =>
                    (
                        !string.IsNullOrEmpty(search) ?
                        (
                            DataContext.DateToString(u.CreationDate).Contains(search.ToLower()) ||
                            DataContext.GetServiceRequestName(u.Ordinal, u.CreationDate).ToLower().Contains(search.ToLower()) ||
                            u.Topic.ToLower().Contains(search.ToLower()) ||
                            u.Description.ToLower().Contains(search.ToLower()) ||
                            (u.Customer != null && u.Customer.User.Name.ToLower().Contains(search.ToLower())) ||
                            (u.Employee != null && u.Employee.User.Name.ToLower().Contains(search.ToLower())) ||
                            (u.PartnerCompany != null && u.PartnerCompany.Name.ToLower().Contains(search.ToLower()))
                        ) : true
                    )
                    && (
                        type != null && type != 0 ? u.RequestType == type : true
                    )
                    && (
                        submitType != null && submitType != 0 ? u.SubmitType == submitType : true
                    )
                    && (
                        status != null && status != 0 ? u.Status == status : true
                    )
                );

            var sql = query.ToQueryString(); 

            query = ApplySorting(query, sortColumn, sortOrder); 

            return await query
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select(u => new ServiceRequestModel //najpierw zwroc model, bo inaczej beda nulle w polach wyliczeniowych (np GetServiceRequestName)
                {
                    CreationDate = u.CreationDate,
                    Topic = u.Topic,
                    Description = u.Description,
                    Customer = u.Customer,
                    PartnerCompany = u.PartnerCompany,
                    Employee = u.Employee,
                    Ordinal = u.Ordinal,
                    RequestType = u.RequestType,
                    SubmitType = u.SubmitType,
                    Status = u.Status,
                })
                .Select(u => new ServiceRequestListDto
                {
                    Id = u.Id,
                    Date = u.CreationDate,
                    Name = DataContext.GetServiceRequestName(u.Ordinal, u.CreationDate),
                    Topic = u.Topic,
                    Description = u.Description,
                    Customer = u.Customer != null ? u.Customer.User.Name : string.Empty,
                    Company = u.PartnerCompany != null ? u.PartnerCompany.Name : string.Empty,
                    Employee = u.Employee != null ? u.Employee.User.Name : string.Empty,
                    Type = DataContext.GetServiceRequestType(u.RequestType),
                    SubmitType = DataContext.GetServiceRequestSubmitType(u.SubmitType),
                    Status = DataContext.GetServiceRequestStatus(u.Status),
                })
                .ToListAsync();                    
        }

        private IQueryable<ServiceRequestModel> ApplySorting(IQueryable<ServiceRequestModel> query, string? sortColumn, string? sortOrder)
        {
            //var sql = query.ToQueryString();    

            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);
            sortColumn = sortColumn ?? nameof(ServiceRequestListDto.Date);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Date), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.CreationDate, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Name), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestNameSorting(u.Ordinal, u.CreationDate), isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Customer), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Customer!.User.Name, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Company), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.PartnerCompany!.Name, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Employee), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Employee!.User.Name, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Type), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestType(u.RequestType), isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.SubmitType), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestSubmitType(u.SubmitType), isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Status), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestStatus(u.Status), isDescending);

            return query.OrderBy(sortColumn, isDescending);
        }

        //public async Task<EmployeeDto?> GetSingle(int id)
        //{
        //    var model = await _context.Employees
        //        .Include(u => u.User)
        //        .SingleOrDefaultAsync(u => u.Id == id);                

        //    return model == null ? null : new EmployeeDto
        //    {
        //        Id = model.Id,
        //        Type = model.Type,
        //        Login = model.User.Login,
        //        Name = model.User.Name,
        //        PhoneNumber = model.User.PhoneNumber,
        //        Email = model.User.Email,
        //        IsMailing = model.IsMailing
        //    };
        //}

        //public async Task<ResponseModel> Add(EmployeeDto dto)
        //{
        //    var model = await _context.Employees
        //       .FirstOrDefaultAsync(u =>
        //            u.User.IsActive &&
        //            (
        //                u.User.Login.ToLower().Equals(dto.Login.ToLower()) ||
        //                u.User.Email.ToLower().Equals(dto.Email.ToLower()) ||
        //                u.User.PhoneNumber.ToLower().Equals(dto.PhoneNumber.ToLower())
        //            )
        //        );

        //    if (model != null)
        //    {
        //        _logger.LogWarning($"Employee {dto.Login} already exists");

        //        return new ResponseModel 
        //        { 
        //            StatusCode = HttpStatusCode.Conflict
        //        };
        //    }

        //    model = new EmployeeModel
        //    {
        //        User = new UserModel
        //        {
        //            Login = dto.Login,
        //            Name = dto.Name,
        //            PhoneNumber = dto.PhoneNumber,
        //            Email = dto.Email,
        //            IsActive = true
        //        },
        //        Type = dto.Type,
        //        IsMailing = dto.IsMailing
        //    };

        //    _context.Employees.Add(model);

        //    var result = await _context.SaveChangesAsync();

        //    return new ResponseModel
        //    {
        //        Id = result <= 0 ? 0 : model.Id,
        //        StatusCode = result <= 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK
        //    };
        //}

        //public async Task<ResponseModel> Update(EmployeeDto dto)
        //{
        //    var model = await _context.Employees
        //        .FirstOrDefaultAsync(u =>
        //            u.Id != dto.Id && 
        //            u.User.IsActive &&
        //            (
        //                u.User.Login.ToLower().Equals(dto.Login.ToLower()) ||
        //                u.User.Email.ToLower().Equals(dto.Email.ToLower()) ||
        //                u.User.PhoneNumber.ToLower().Equals(dto.PhoneNumber.ToLower())
        //            )
        //        );

        //    if (model != null)
        //    {
        //        _logger.LogWarning($"Employee {dto.Login} login, email, phone number conflict");

        //        return new ResponseModel
        //        {
        //            StatusCode = HttpStatusCode.Conflict
        //        };
        //    }

        //    model = await _context.Employees
        //       .Include(u => u.User)
        //       .SingleOrDefaultAsync(u => u.Id == dto.Id);

        //    if (model == null)
        //    {
        //        _logger.LogWarning($"Employee id: {dto.Id} not found");

        //        return new ResponseModel
        //        {
        //            StatusCode = HttpStatusCode.NotFound
        //        };
        //    }            

        //    model.User.Login = dto.Login;
        //    model.User.Name = dto.Name;
        //    model.User.PhoneNumber = dto.PhoneNumber;
        //    model.User.Email = dto.Email;
        //    model.Type = dto.Type;
        //    model.IsMailing = dto.IsMailing;

        //    var result = await _context.SaveChangesAsync();

        //    return new ResponseModel
        //    {
        //        Id = model.Id,
        //        StatusCode = HttpStatusCode.OK
        //    };
        //}

        //public async Task<Boolean> Delete(int id)
        //{
        //    var model = await _context.Employees
        //        .Include(u => u.User)
        //        .SingleOrDefaultAsync(u => u.Id == id && u.User.IsActive);

        //    if (model == null)
        //    {
        //        _logger.LogWarning($"Employee id: {id} not found");
        //        return false;
        //    }

        //    model.User.IsActive = false;

        //    var result = await _context.SaveChangesAsync();

        //    return result > 0;
        //}

        //public async Task<Boolean> DeleteAll()
        //{
        //    var result = await _context.Database.ExecuteSqlRawAsync(@"
        //        update Users 
        //        set IsActive = 0 
        //        from Users 
        //        inner join Employees on Users.Id = Employees.UserId
        //    ");

        //    return result > 0;            
        //}              
    }
}
