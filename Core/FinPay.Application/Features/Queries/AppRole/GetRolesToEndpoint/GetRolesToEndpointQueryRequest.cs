using MediatR;

namespace FinPay.Application.Features.Queries.AppRole.GetRolesToEndpoint
{
    public class GetRolesToEndpointQueryRequest:IRequest<GetRolesToEndpointQueryResponse>
    {
            public string Code { get; set; }
            public string Menu { get; set; }
    }
}