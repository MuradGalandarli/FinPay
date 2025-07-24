using FinPay.Application.Features.Commands.Payment.UserAccount;
using FinPay.Application.Features.Queries.GetCardBalanceByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentetion.Controllers
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
        public async Task<IActionResult> CreateUserAccount([FromBody]UserAccountCommandRequest userAccountCommandRequest)
        {
            UserAccountCommandResponse userAccountCommandResponse = await _mediator.Send(userAccountCommandRequest); 
            return Ok(userAccountCommandResponse);
        }
        [HttpGet("GetCardBalanceByAccountId")]
        public async Task<IActionResult> GetCardBalanceByAccountId([FromQuery]GetCardBalanceByUserIdQueryRequest userIdQueryRequest)
        {
            GetCardBalanceByUserIdQueryResponse getCardBalanceByUserIdQueryResponse = await _mediator.Send(userIdQueryRequest);
            return Ok(getCardBalanceByUserIdQueryResponse);
        }


    }
}
