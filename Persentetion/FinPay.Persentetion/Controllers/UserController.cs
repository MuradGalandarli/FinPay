using FinPay.Application.Consts;
using FinPay.Application.CustomAttributes;
using FinPay.Application.Features.Commands.AppUser.AssingRoleToUser;
using FinPay.Application.Features.Commands.AppUser.ResetPassword;
using FinPay.Application.Features.Commands.AppUser.VerifyResetToken;
using FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint;
using FinPay.Application.Features.Queries.AppUser.GetUser;
using FinPay.Application.Features.UpdatePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Admin")]
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
        public async Task<IActionResult>AssingRoleToUser([FromBody]AssignRoleToUserCommandRequest assingRoleToUserCommandRequest)
        {
            AssignRoleToUserCommandResponse assingRoleToUserCommandRespose = await _mediator.Send(assingRoleToUserCommandRequest);
            return Ok(assingRoleToUserCommandRespose);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequest resetPasswordCommandRequest)
        {
           await _mediator.Send(resetPasswordCommandRequest);
            return Ok();
        }
        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken(VerifyResetTokenCommandRequest verifyResetTokenCommandRequest)
        {
           VerifyResetTokenCommandResponse verifyResetTokenCommandResponse = await _mediator.Send(verifyResetTokenCommandRequest);
            return Ok(verifyResetTokenCommandResponse);
        }
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
             await _mediator.Send(updatePasswordCommandRequest);
            return Ok();
        }

    }
}
