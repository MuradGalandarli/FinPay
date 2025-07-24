using FinPay.Application.Service;
using FinPay.Application.Service.Configurations;
using FinPay.Persentetion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentetion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServicesController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ApplicationServicesController> _logger;
        
        public ApplicationServicesController(IApplicationService applicationService, ILogger<ApplicationServicesController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
           
        }

        [HttpGet]
        public async Task<IActionResult> TestAssabely()
        {
            var getAuthorizeDefinitionEndponit = _applicationService.GetAuthorizeDefinitionEndponit(typeof(Program));
            return Ok(getAuthorizeDefinitionEndponit);

        }

    }
}
