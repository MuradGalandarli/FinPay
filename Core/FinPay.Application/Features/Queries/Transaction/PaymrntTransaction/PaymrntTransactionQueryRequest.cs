using MediatR;

namespace FinPay.Application.Features.Queries.Transaction.PaymrntTransaction
{
    public class PaymrntTransactionQueryRequest:IRequest<List<PaymrntTransactionQueryRespose>>
    {
        public int UserAccountId { get; set; }
    }
}