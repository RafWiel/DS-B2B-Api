using WebApiService.Filters;
using WebApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WebApiService.Interfaces;

namespace WebApiService.Controllers
{
    [ApiController]
    [Route("api/install")]
    public class InstallController : ControllerBase
    {
        private readonly IInstallService _service;
        private readonly ILogger<InstallController> _logger;

        public InstallController(IInstallService service, ILogger<InstallController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]        
        public async Task<ActionResult> Install()
        {
            var result = await _service.Install();

            return result ? Ok() : BadRequest();
        }        
    }
}

