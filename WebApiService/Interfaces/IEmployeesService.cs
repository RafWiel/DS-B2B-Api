using WebApiService.DataTransferObjects;
using WebApiService.Models;

namespace WebApiService.Interfaces
{
    public interface IEmployeesService
    {
        Task<List<EmployeeDto>> Get();
    }
}
