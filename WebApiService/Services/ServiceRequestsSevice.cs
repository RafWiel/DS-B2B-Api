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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApiService.Services
{
    public class ServiceRequestsService : IServiceRequestsService
    {
        private readonly DataContext _context;
        private readonly ILogger<IServiceRequestsService> _logger;

        private class ExtendedServiceRequestModel
        {
            public ServiceRequestModel Request { get; set; }
            public string CompanyName { get; set; }
        }

        public ServiceRequestsService(DataContext context, ILogger<IServiceRequestsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ServiceRequestListDto>> Get(
            string? search,
            DateTime? start, 
            DateTime? end,
            int? employee,
            int? type,
            int? submitType,
            int? status,
            string? sortColumn,
            string? sortOrder,
            int? page)
        {

            var query =
                from request in _context.ServiceRequests
                join c in _context.Companies on
                    request.Customer.CompanyModelId equals c.Id into companies
                from company in companies.DefaultIfEmpty()
                where (
                        !string.IsNullOrEmpty(search) ?
                        (
                            DataContext.DateToString(request.CreationDate).ToLower().Contains(search.ToLower()) ||
                            DataContext.GetServiceRequestName(request.Ordinal, request.CreationDate).ToLower().Contains(search.ToLower()) ||
                            request.Topic.ToLower().Contains(search.ToLower()) ||
                            request.Description.ToLower().Contains(search.ToLower()) ||
                            (request.Customer != null && request.Customer.User.Name.ToLower().Contains(search.ToLower())) ||
                            (request.Employee != null && request.Employee.User.Name.ToLower().Contains(search.ToLower())) ||
                            (company.Name != null && company.Name.ToLower().Contains(search.ToLower()))
                        ) : true
                    )
                    && (
                        start != null ? request.CreationDate >= start : true
                    )
                    && (
                        end != null ? request.CreationDate <= end.Value.AddDays(1) : true
                    )
                    && (
                        employee != null ? request.EmployeeId == employee : true
                    )
                    && (
                        type != null && type != 0 ? request.RequestType == type : true
                    )
                    && (
                        submitType != null && submitType != 0 ? request.SubmitType == submitType : true
                    )
                    && (
                        status != null && status != 0 ? (request.Status & status) != 0 : true
                    )

                select new ExtendedServiceRequestModel
                {
                    Request = request,
                    CompanyName = company.Name ?? string.Empty,
                };            

            query = ApplySorting(query, sortColumn, sortOrder);

            return await query                
                .Skip(50 * ((page ?? 1) - 1))
                .Take(50)
                .Select((u) => new ServiceRequestListDto
                {
                    Id = u.Request.Id,
                    Date = u.Request.CreationDate,
                    Name = DataContext.GetServiceRequestName(u.Request.Ordinal, u.Request.CreationDate),
                    Topic = u.Request.Topic,
                    Description = u.Request.Description,
                    Customer = u.Request.Customer != null ? u.Request.Customer.User.Name : string.Empty,
                    Company = u.CompanyName,
                    Employee = u.Request.Employee != null ? u.Request.Employee.User.Name : string.Empty,
                    Type = DataContext.GetServiceRequestType(u.Request.RequestType),
                    SubmitType = DataContext.GetServiceRequestSubmitType(u.Request.SubmitType),
                    Status = DataContext.GetServiceRequestStatus(u.Request.Status),
                    Ordinal = u.Request.Ordinal,
                    TypeNum = u.Request.RequestType,
                    SubmitTypeNum = u.Request.SubmitType,
                    StatusNum = u.Request.Status,
                })
                .ToListAsync();
        }        

        private IQueryable<ExtendedServiceRequestModel> ApplySorting(IQueryable<ExtendedServiceRequestModel> query, string? sortColumn, string? sortOrder)
        {
            //var sql = query.ToQueryString();    

            var isDescending = (sortOrder ?? string.Empty).Equals("desc", StringComparison.OrdinalIgnoreCase);
            sortColumn = sortColumn ?? nameof(ServiceRequestListDto.Date);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Date), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Request.CreationDate, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Name), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestNameSorting(u.Request.Ordinal, u.Request.CreationDate), isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Topic), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Request.Topic, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Customer), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Request.Customer!.User.Name, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Company), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.CompanyName, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Employee), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => u.Request.Employee!.User.Name, isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Type), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestTypeSorting(u.Request.RequestType), isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.SubmitType), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestSubmitTypeSorting(u.Request.SubmitType), isDescending);

            if (sortColumn.Equals(nameof(ServiceRequestListDto.Status), StringComparison.OrdinalIgnoreCase))
                return query.OrderByWithDirection(u => DataContext.GetServiceRequestStatusSorting(u.Request.Status), isDescending);

            return query;
        }

        public async Task<ServiceRequestDto?> GetSingle(int id)
        {
            var model = await _context.ServiceRequests                
                .SingleOrDefaultAsync(u => u.Id == id);

            return model == null ? null : new ServiceRequestDto
            {
                Id = model.Id,
                CreationDate = model.CreationDate,
                ClosureDate = model.ClosureDate,
                ReminderDate = model.ReminderDate,
                Ordinal = model.Ordinal,
                CompanyName = model.CompanyName,
                Topic = model.Topic,
                Description = model.Description,
                Status = ServiceRequestStatus.GetText(model.Status),
                RequestType = ServiceRequestType.GetText(model.RequestType),
                SubmitType = ServiceRequestSubmitType.GetText(model.SubmitType),
                Invoice = model.Invoice
            };
        }

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

        public async Task<Boolean> Delete(int id)
        {
            var model = await _context.ServiceRequests
                .SingleOrDefaultAsync(u => u.Id == id);

            if (model == null)
            {
                _logger.LogWarning($"Service request id: {id} not found");
                return false;
            }

            _context.ServiceRequests.Remove(model);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Boolean> DeleteAll()
        {
            var result = await _context.Database.ExecuteSqlRawAsync(@"
                delete from ServiceRequests                
            ");

            return result > 0;
        }
    }
}
