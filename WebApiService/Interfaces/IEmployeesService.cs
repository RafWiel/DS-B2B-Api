using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface IEmployeesService
    {
        Task<List<EmployeeDto>> Get(string? search, int? type, string? sortColumn, string? sortOrder, int? page);
        Task<Boolean> Delete(int id);
        Task<Boolean> DeleteAll();
    }
}
