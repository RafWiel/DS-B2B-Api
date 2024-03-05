using System.Net;
using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface ICompaniesService
    {
        //Task<List<CompanyListDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page);
        //Task<CustomerDto?> GetSingle(int id);
        Task<ResponseModel> Add(CompanyDto dto);
        //Task<ResponseModel> Update(CustomerDto dto);
        //Task<Boolean> Delete(int id);
        //Task<Boolean> DeleteAll();
    }
}
