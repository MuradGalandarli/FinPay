using MediatR;

namespace FinPay.Application.Features.Queries.AppUser.GetUser
{
    public class GetAllUserQueryRequest:IRequest<GetAllUserQueryResponse>
    {
        public int Size { get; set; }
        public int Page { get; set; }
    }
}