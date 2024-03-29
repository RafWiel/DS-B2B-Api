﻿using System.Net;
using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface ICompaniesService
    {
        Task<List<CompanyListDto>> Get(string? search, string? sortColumn, string? sortOrder, int? page);
        Task<List<ListDto>> GetList();
        Task<CompanyDto?> GetSingle(int id);
        Task<IdResponseModel> Add(CompanyDto dto);
        Task<IdResponseModel> Update(CompanyDto dto);
        Task<Boolean> Delete(int id);
        Task<Boolean> DeleteAll();
    }
}
