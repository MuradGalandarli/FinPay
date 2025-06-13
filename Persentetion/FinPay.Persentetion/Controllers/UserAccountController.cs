using FinPay.Application.Features.Commands.Payment.UserAccount;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateUserAccount")]
        public async Task<IActionResult> CreateUserAccount(UserAccountCommandRequest userAccountCommandRequest)
        {
            UserAccountCommandResponse userAccountCommandResponse = await _mediator.Send(userAccountCommandRequest); 
            return Ok(userAccountCommandResponse);
        }


    }
}
