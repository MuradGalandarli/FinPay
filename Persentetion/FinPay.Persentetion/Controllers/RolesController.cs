using FinPay.Application.Features.Commands.AppRole.CreateRole;
using FinPay.Application.Features.Commands.AppRole.DeleteRole;
using FinPay.Application.Features.Commands.AppRole.UpdateRole;
using FinPay.Application.Features.Queries.AppRole.GetRoleById;
using FinPay.Application.Features.Queries.AppRole.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentetion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes ="Admin")]
  
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles([FromQuery]GetRolesQueryRequest getRoleQueryRequest)
        {
            GetRoleQueryResponse getRoleQueryResponse = await _mediator.Send(getRoleQueryRequest);
            return Ok(getRoleQueryResponse);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute]GetRoleByIdQueryRequest getRoleByIdQueryRequest)
        {
          GetRoleByIdQueryResponse getRoleByIdQueryResponse = await _mediator.Send(getRoleByIdQueryRequest);
            return Ok(getRoleByIdQueryResponse);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleCommandRequest createRoleCommandRequest)
        {
            CreateRoleCommandResponse createRoleCommandResponse = await _mediator.Send(createRoleCommandRequest);
            return Ok(createRoleCommandResponse);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommandRequest updateRoleCommandRequest)
        {
            UpdateRoleCommandResponse updateRoleCommandResponse = await _mediator.Send(updateRoleCommandRequest);
            return Ok(updateRoleCommandResponse);
        }
        [HttpDelete("{Name}")]
        public async Task<IActionResult> DeleteRols([FromRoute]DeleteRoleCommandRequest deleteRoleCommandRequest)
        {
            DeleteRoleCommandResponse deleteRoleCommandResponse = await _mediator.Send(deleteRoleCommandRequest);
            return Ok(deleteRoleCommandResponse);
        }

    }
}
