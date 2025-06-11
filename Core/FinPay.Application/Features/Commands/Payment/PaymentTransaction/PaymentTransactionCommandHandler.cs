using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.Payment.PaymentTransaction
{
    public class PaymentTransactionCommandHandler : IRequestHandler<PaymentTransactionCommandRequest, PaymentTransactionCommandResponse>
    {
        private readonly IPaymentTransaction _paymentTransaction;

        public PaymentTransactionCommandHandler(IPaymentTransaction paymentTransaction)
        { 
            _paymentTransaction = paymentTransaction;
        }

        public async Task<PaymentTransactionCommandResponse> Handle(PaymentTransactionCommandRequest request, CancellationToken cancellationToken)
        {
           string paymentTransactionCommandResponse = await _paymentTransaction.CreatePayment(request.Amount,request.UserId);

            return new() { AccessToken = paymentTransactionCommandResponse };
        }
    }
}
