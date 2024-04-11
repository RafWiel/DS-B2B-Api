using System.Net;
using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface IServiceRequestsService
    {
        Task<List<ServiceRequestListDto>> Get(
            string? search, 
            DateTime? start,
            DateTime? end,
            int? employee,
            int? type, 
            int? submitType, 
            int? status, 
            string? sortColumn, 
            string? sortOrder, 
            int? page);
        //Task<EmployeeDto?> GetSingle(int id);
        //Task<ResponseModel> Add(EmployeeDto dto);
        //Task<ResponseModel> Update(EmployeeDto dto);
        Task<Boolean> Delete(int id);
        Task<Boolean> DeleteAll();
    }
}
