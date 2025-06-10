using FinPay.Application.Features.Commands.Payment.CaptureOrder;
using FinPay.Application.Features.Commands.Payment.PaymentTransaction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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



    }
}
