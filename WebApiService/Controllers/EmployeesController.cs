using WebApiService.Filters;
using WebApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WebApiService.Interfaces;
using WebApiService.DataTransferObjects;

namespace WebApiService.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _service;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeesService service, ILogger<EmployeesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]        
        public async Task<ActionResult<List<EmployeeDto>>> Get(
            string? search, 
            int? type,
            [FromQuery(Name = "sort-column")] string? sortColumn,
            [FromQuery(Name = "sort-order")] int? sortOrder, 
            int? page)
        {
            var items = await _service.Get(search, type, sortColumn, sortOrder, page);
            if (items == null)
                return NotFound();

            return Ok(items);
        }        
    }
}

