using MediatR;

namespace FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint
{
    public class AssingRoleEndpointCommandRequest:IRequest<AssingRoleEndpointCommandResponse>
    {
        public string[] Role { get; set; }
        public string EndpointCode { get; set; }
        public string Menu { get; set; }
        public Type? type { get; set; }
    }
}