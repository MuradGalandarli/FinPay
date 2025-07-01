using FinPay.Application.Service;
using FluentValidation;
using MediatR;

namespace FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint
{
    public class AssingRoleEndpointCommandHandler : IRequestHandler<AssingRoleEndpointCommandRequest, AssingRoleEndpointCommandResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;
        private readonly IValidator<AssingRoleEndpointCommandRequest> _validator;

        public AssingRoleEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService, IValidator<AssingRoleEndpointCommandRequest> validator)
        {
            _authorizationEndpointService = authorizationEndpointService;
            _validator = validator;
        }
        public async Task<AssingRoleEndpointCommandResponse> Handle(AssingRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult);

            await _authorizationEndpointService.AssingRoleEndpointAsync(request.Role, request.Menu, request.EndpointCode, request.type);
                return new();
           
        }
    }
}
