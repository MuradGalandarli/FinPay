using MediatR;

namespace FinPay.Application.Features.Queries.AppRole.GetRoles
{
    public class GetRolesQueryRequest:IRequest<GetRoleQueryResponse>
    {
        public int Size { get; set; }
        public int Page { get; set; }
    }
}