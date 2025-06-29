using FinPay.Application.Service.Payment;
using FluentValidation;
using MediatR;

namespace FinPay.Application.Features.Commands.Payment.PaymentTransaction
{
    public class PaymentTransactionCommandHandler : IRequestHandler<PaymentTransactionCommandRequest, PaymentTransactionCommandResponse>
    {
        private readonly IPaymentTransaction _paymentTransaction;
        private readonly IValidator<PaymentTransactionCommandRequest> _validator;

        public PaymentTransactionCommandHandler(IPaymentTransaction paymentTransaction, IValidator<PaymentTransactionCommandRequest> validator)
        {
            _paymentTransaction = paymentTransaction;
            _validator = validator;
        }

        public async Task<PaymentTransactionCommandResponse> Handle(PaymentTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            if (_validator.Validate(request).IsValid)
            {
                string paymentTransactionCommandResponse = await _paymentTransaction.CreatePayment(request.Amount, request.UserAccountId);

                return new() { AccessToken = paymentTransactionCommandResponse };
            }
            throw new Exceptions.ValidationException();
        }
    }
}
