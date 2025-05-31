using MediatR;

namespace FinPay.Application.Features.Queries.AppRole.GetRoleById
{
    public class GetRoleByIdQueryRequest:IRequest<GetRoleByIdQueryResponse>
    {
        public string id { get; set; }
    }
}