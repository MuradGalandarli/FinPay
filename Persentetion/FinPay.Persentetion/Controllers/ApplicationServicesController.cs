using FinPay.Application.Service.Configurations;
using FinPay.Persentetion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServicesController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult TestAssabely()
        {
            var getAuthorizeDefinitionEndponit = _applicationService.GetAuthorizeDefinitionEndponit(typeof(Program));
            return Ok(getAuthorizeDefinitionEndponit);
        }

    }
}
