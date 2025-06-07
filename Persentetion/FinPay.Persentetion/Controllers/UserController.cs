using FinPay.Application.Consts;
using FinPay.Application.CustomAttributes;
using FinPay.Application.Features.Commands.AppUser.AssingRoleToUser;
using FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint;
using FinPay.Application.Features.Queries.AppUser.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Reading, Menu = AuthorizeDefinitionConstants.ControllerUser, Definition = "Get all User")]
        public async Task<IActionResult> GetAllUser([FromQuery] GetAllUserQueryRequest getAllUserQueryRequest)
        {
           GetAllUserQueryResponse getAllUserQueryResponse = await _mediator.Send(getAllUserQueryRequest);
            return Ok(getAllUserQueryResponse);
        }


        [HttpPost("assing-role-to-user")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Writing,Menu = AuthorizeDefinitionConstants.ControllerUser,Definition ="Assing Role To User")]
        public async Task<IActionResult>AssingRoleToUser(AssingRoleToUserCommandRequest assingRoleToUserCommandRequest)
        {
            AssingRoleToUserCommandRespose assingRoleToUserCommandRespose = await _mediator.Send(assingRoleToUserCommandRequest);
            return Ok(assingRoleToUserCommandRespose);
        }
    }
}
