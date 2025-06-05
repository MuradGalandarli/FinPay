using FinPay._Infrastructure.Service.Configurations;
using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Application.Service.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinPay.Domain.Entity;
using Microsoft.AspNetCore.Authentication;
using FinPay.Application.Service;
using FinPay.Persentetion;
using MediatR;
using FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint;
using FinPay.Application.Features.Queries.AppRole.GetRolesToEndpoint;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AssingRoleEndpointAsync([FromBody]AssingRoleEndpointCommandRequest assingRoleEndpointCommandRequest)
        {
            assingRoleEndpointCommandRequest.type = typeof(Program);
          AssingRoleEndpointCommandResponse assingRoleEndpointCommandResponse = await _mediator.Send(assingRoleEndpointCommandRequest);
            return Ok(assingRoleEndpointCommandResponse);    
        }
        [HttpGet("GetRolesToEndpoint")]
        public async Task<IActionResult> GetRolesToEndpointAsync([FromQuery]GetRolesToEndpointQueryRequest getRolesToEndpointQueryRequest)
        {
           GetRolesToEndpointQueryResponse getRolesToEndpointQueryResponse = await _mediator.Send(getRolesToEndpointQueryRequest);
            return Ok(getRolesToEndpointQueryResponse);
        }
    }
}
