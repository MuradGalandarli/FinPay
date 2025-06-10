using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.Payment.CaptureOrder
{
    public class CaptureOrderCommandHandler : IRequestHandler<CaptureOrderCommandRequest, CaptureOrderCommandResponse>
    {
        private readonly IPaymentTransaction _paymentTransaction;

        public CaptureOrderCommandHandler(IPaymentTransaction paymentTransaction)
        {
            _paymentTransaction = paymentTransaction;
        }

        public async Task<CaptureOrderCommandResponse> Handle(CaptureOrderCommandRequest request, CancellationToken cancellationToken)
        {
            string response = await _paymentTransaction.CaptureOrderAsync(request.Id);
            return new()
            {
                Status = response,
            };
        }
    }
}
