using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface IEmployeesService
    {
        Task<List<EmployeeDto>> Get(string? search, int? type, string? sortColumn, int? sortOrder, int? page);
    }
}
