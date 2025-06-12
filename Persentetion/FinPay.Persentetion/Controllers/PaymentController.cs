using FinPay.Application.Features.Commands.CardTransaction;
using FinPay.Application.Features.Commands.Payment.CaptureOrder;
using FinPay.Application.Features.Commands.Payment.PaymentTransaction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FinPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PaymentTransaction(PaymentTransactionCommandRequest paymentTransactionCommandRequest)
        {
            PaymentTransactionCommandResponse paymentTransactionCommandResponse = await _mediator.Send(paymentTransactionCommandRequest);
            return Ok(paymentTransactionCommandResponse);

        }
        [HttpPost("CaptureOrder")]
        public async Task<IActionResult> CaptureOrderAsync([FromBody]CaptureOrderCommandRequest captureOrderCommandRequest)
        {
           CaptureOrderCommandResponse captureOrderCommandResponse = await _mediator.Send(captureOrderCommandRequest);
            return Ok(captureOrderCommandResponse);
        }
        [HttpPost("CardTransaction")]
        public async Task<IActionResult> CardTransaction([FromBody] CardTransactionCommandRequest cardTransactionCommandRequest)
        {
            CardTransactionCommandResponse cardTransactionCommandResponse = await _mediator.Send(cardTransactionCommandRequest);
            return Ok(cardTransactionCommandResponse);
        }



    }
}
