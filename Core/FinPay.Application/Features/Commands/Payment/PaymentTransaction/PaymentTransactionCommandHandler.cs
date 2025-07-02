using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using FluentValidation;
using MediatR;
using System.Diagnostics;

namespace FinPay.Application.Features.Commands.Payment.PaymentTransaction
{
    public class PaymentTransactionCommandHandler : IRequestHandler<PaymentTransactionCommandRequest, PaymentTransactionCommandResponse>
    {
        private readonly IPaymentTransaction _paymentTransaction;
        private readonly IValidator<PaymentTransactionCommandRequest> _validator;
        private readonly IMetricsService _metricsService;

        public PaymentTransactionCommandHandler(IPaymentTransaction paymentTransaction, IValidator<PaymentTransactionCommandRequest> validator, IMetricsService metricsService)
        {
            _paymentTransaction = paymentTransaction;
            _validator = validator;
            _metricsService = metricsService;
        }

        public async Task<PaymentTransactionCommandResponse> Handle(PaymentTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("CreatePayment");
            var sw = Stopwatch.StartNew();
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                string paymentTransactionCommandResponse = await _paymentTransaction.CreatePayment(request.Amount, request.UserAccountId);

                return new() { AccessToken = paymentTransactionCommandResponse };
            }
            finally
            {
                sw.Stop();
                _metricsService.ObserveHistogram("CreatePayment_duration_seconds", sw.Elapsed.TotalSeconds);
            }
           
           
        }
    }
}
