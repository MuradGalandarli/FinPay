using MediatR;

namespace FinPay.Application.Features.Queries.GetCardBalanceByUserId
{
    public class GetCardBalanceByUserIdQueryRequest:IRequest<GetCardBalanceByUserIdQueryResponse>
    {
        public int UserAccountId { get; set; }
    }
}