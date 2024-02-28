using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface IEmployeesService
    {
        Task<List<EmployeeListDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page);
        Task<EmployeeDto?> GetSingle(int id);
        Task<Boolean> Delete(int id);
        Task<Boolean> DeleteAll();
    }
}
