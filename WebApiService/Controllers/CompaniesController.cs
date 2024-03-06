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
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesService _service;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompaniesService service, ILogger<CompaniesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyListDto>>> Get(
            string? search,
            [FromQuery(Name = "sort-column")] string? sortColumn,
            [FromQuery(Name = "sort-order")] string? sortOrder,
            int? page)
        {
            var items = await _service.Get(search, sortColumn, sortOrder, page);
            if (items == null)
                return NotFound();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDto>> GetSingle(int id)
        {
            var dto = await _service.GetSingle(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<IdResponseDto>> Add(CompanyDto dto)
        {
            var result = await _service.Add(dto);

            if (result.StatusCode == HttpStatusCode.OK)
                return Ok(new IdResponseDto
                {
                    Id = result.Id
                });

            if (result.StatusCode == HttpStatusCode.Conflict)
                return Conflict();

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<IdResponseDto>> Update(CompanyDto dto)
        {
            var result = await _service.Update(dto);

            if (result.StatusCode == HttpStatusCode.OK)
                return Ok(new IdResponseDto
                {
                    Id = result.Id
                });

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound();

            if (result.StatusCode == HttpStatusCode.Conflict)
                return Conflict();

            return BadRequest();
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

