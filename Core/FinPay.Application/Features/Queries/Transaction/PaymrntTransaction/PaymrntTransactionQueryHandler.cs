using FinPay.Application.Service.Payment;
using MediatR;


namespace FinPay.Application.Features.Queries.Transaction.PaymrntTransaction
{
    public class PaymrntTransactionQueryHandler : IRequestHandler<PaymrntTransactionQueryRequest, List<PaymrntTransactionQueryRespose>>
    {
        private readonly IPaymentTransaction _paymentTransaction;

        public PaymrntTransactionQueryHandler(IPaymentTransaction paymentTransaction)
        {
            _paymentTransaction = paymentTransaction;
        }

        public async Task<List<PaymrntTransactionQueryRespose>> Handle(PaymrntTransactionQueryRequest request, CancellationToken cancellationToken)
        {
           var transaction = await _paymentTransaction.GetTransactionsByUserAccountId(request.UserAccountId);
           var paymrntTransactionQueryRespose = transaction.Select(x => new PaymrntTransactionQueryRespose()
            {
                UserAccountId = request.UserAccountId,
                Amount = x.Amount,
                CreateAt = x.CreateAt,
                IsPayoutSent = x.IsPayoutSent,
                PaypalEmail = x.PaypalEmail,
                Status = x.Status, 
                
            });
            return paymrntTransactionQueryRespose.ToList();
        }
    }
}
