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
        public async Task<ActionResult<List<EmployeeListDto>>> Get(
            string? search, 
            int? type,
            [FromQuery(Name = "sort-column")] string? sortColumn,
            [FromQuery(Name = "sort-order")] string? sortOrder, 
            int? page)
        {
            var items = await _service.Get(search, type, sortColumn, sortOrder, page);
            if (items == null)
                return NotFound();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeModel>> Get(int id)
        {
            Thread.Sleep(5000);
            var model = await _service.Get(id);
            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);

            return result ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            var result = await _service.DeleteAll();

            return result ? Ok() : BadRequest();
        }
    }
}

