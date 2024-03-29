﻿using System.Net;
using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface ICustomersService
    {
        Task<List<CustomerListDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page);
        Task<CustomerDto?> GetSingle(int id);
        Task<IdResponseModel> Add(CustomerDto dto);
        Task<IdResponseModel> Update(CustomerDto dto);
        Task<Boolean> Delete(int id);
        Task<Boolean> DeleteAll();
        Task<Boolean> DeleteAllCompany(int companyId);
    }
}
