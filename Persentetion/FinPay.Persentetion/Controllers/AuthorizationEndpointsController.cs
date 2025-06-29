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
using FinPay.Persistence.Repositoryes.Endpoint;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEndpointReadRepository _endpointReadRepository;
       
        public AuthorizationEndpointsController(IMediator mediator, IEndpointReadRepository endpointReadRepository)
        {
            _mediator = mediator;
            _endpointReadRepository = endpointReadRepository;
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
        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            var test = _endpointReadRepository.Table;
            
            return Ok(test);
        }
    }
}
