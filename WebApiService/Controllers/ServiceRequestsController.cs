using WebApiService.Filters;
using WebApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WebApiService.Interfaces;
using WebApiService.DataTransferObjects;
using System.Net;

namespace WebApiService.Controllers
{
    [ApiController]
    [Route("api/service-requests")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestsService _service;
        private readonly ILogger<ServiceRequestsController> _logger;

        public ServiceRequestsController(IServiceRequestsService service, ILogger<ServiceRequestsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]        
        public async Task<ActionResult<List<ServiceRequestListDto>>> Get(
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

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<EmployeeDto>> GetSingle(int id)
        //{            
        //    var dto = await _service.GetSingle(id);
        //    if (dto == null)
        //        return NotFound();

        //    return Ok(dto);
        //}

        //[HttpPost]
        //public async Task<ActionResult<IdResponseDto>> Add(EmployeeDto dto)
        //{
        //    var result = await _service.Add(dto);

        //    if (result.StatusCode == HttpStatusCode.OK)
        //    {
        //        return Ok(new IdResponseDto
        //        {
        //            Id = result.Id
        //        });
        //    }

        //    if (result.StatusCode == HttpStatusCode.Conflict)
        //        return Conflict();

        //    return BadRequest();
        //}

        //[HttpPut]
        //public async Task<ActionResult<IdResponseDto>> Update(EmployeeDto dto)
        //{
        //    var result = await _service.Update(dto);

        //    if (result.StatusCode == HttpStatusCode.OK)
        //    {
        //        return Ok(new IdResponseDto
        //        {
        //            Id = result.Id
        //        });
        //    }

        //    if (result.StatusCode == HttpStatusCode.NotFound)
        //        return NotFound();

        //    if (result.StatusCode == HttpStatusCode.Conflict)
        //        return Conflict();

        //    return BadRequest();
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var result = await _service.Delete(id);

        //    return result ? Ok() : BadRequest();
        //}

        //[HttpDelete]
        //public async Task<ActionResult> DeleteAll()
        //{
        //    var result = await _service.DeleteAll();

        //    return result ? Ok() : BadRequest();
        //}
    }
}

