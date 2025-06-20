using MediatR;

namespace FinPay.Application.Features.Commands.Payment.PaymentTransaction
{
    public class PaymentTransactionCommandRequest:IRequest<PaymentTransactionCommandResponse>
    {
        public decimal Amount { get; set; }
        public int UserAccountId { get; set; }
    }
}