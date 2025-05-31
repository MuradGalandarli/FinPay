using MediatR;

namespace FinPay.Application.Features.Queries.AppRole.GetRoles
{
    public class GetRolesQueryRequest:IRequest<GetRoleQueryResponse>
    {
    }
}