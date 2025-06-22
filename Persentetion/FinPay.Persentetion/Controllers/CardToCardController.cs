using FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction;
using FinPay.Application.Features.Commands.CardToCardTransaction.UpdateTransactionStatusAndPublish;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardToCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardToCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("UpdateTransactionStatusAndPublish")]
        public async Task<IActionResult> UpdateTransactionStatusAndPublish([FromRoute] UpdateTransactionStatusAndPublishCommandRequest updateTransactionStatusAndPublishCommandRequest)
        {
            UpdateTransactionStatusAndPublishCommandResponse updateTransactionStatusAndPublishCommandResponse = await _mediator.Send(updateTransactionStatusAndPublishCommandRequest);
            return Ok(updateTransactionStatusAndPublishCommandResponse);
        }
        [HttpPost("CardTransaction")]
        public async Task<IActionResult> CardTransaction([FromBody] CardTransactionCommandRequest cardTransactionCommandRequest)
        {
            CardTransactionCommandResponse cardTransactionCommandResponse = await _mediator.Send(cardTransactionCommandRequest);
            return Ok(cardTransactionCommandResponse);
        }


    }
}
