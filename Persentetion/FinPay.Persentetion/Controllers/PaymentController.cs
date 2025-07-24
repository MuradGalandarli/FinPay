using FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction;
using FinPay.Application.Features.Commands.Payment.CaptureOrder;
using FinPay.Application.Features.Commands.Payment.PaymentTransaction;
using FinPay.Application.Features.Queries.Transaction.PaymrntTransaction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FinPay.Presentetion.Controllers
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

        [HttpPost("PaymentTransaction")]
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
        

        [HttpGet("GetPaymrntTransactionAccountId")]
        public async Task<IActionResult> GetPaymrntTransactionAccountId([FromQuery] PaymrntTransactionQueryRequest paymrntTransactionQueryRequest )
        {
            List<PaymrntTransactionQueryRespose> paymrntTransactionQueryResposes = await _mediator.Send(paymrntTransactionQueryRequest);
            return Ok(paymrntTransactionQueryResposes);
        }

    }
}
