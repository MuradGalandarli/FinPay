using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint
{
    public class AssingRoleEndpointCommandHandler : IRequestHandler<AssingRoleEndpointCommandRequest, AssingRoleEndpointCommandResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;

        public AssingRoleEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }
        public async Task<AssingRoleEndpointCommandResponse> Handle(AssingRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            await _authorizationEndpointService.AssingRoleEndpointAsync(request.Role, request.Menu, request.EndpointCode, request.type);
            return new();
        }
    }
}
