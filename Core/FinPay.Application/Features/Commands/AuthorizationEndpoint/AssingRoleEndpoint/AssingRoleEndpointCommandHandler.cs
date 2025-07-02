using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using System.Diagnostics;

namespace FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint
{
    public class AssingRoleEndpointCommandHandler : IRequestHandler<AssingRoleEndpointCommandRequest, AssingRoleEndpointCommandResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;
        private readonly IValidator<AssingRoleEndpointCommandRequest> _validator;
        private readonly IMetricsService _metricsService;

        public AssingRoleEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService, IValidator<AssingRoleEndpointCommandRequest> validator, IMetricsService metricsService = null)
        {
            _authorizationEndpointService = authorizationEndpointService;
            _validator = validator;
            _metricsService = metricsService;
        }
        public async Task<AssingRoleEndpointCommandResponse> Handle(AssingRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("AssignRoleEndpoint");
            var sw = Stopwatch.StartNew();
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                await _authorizationEndpointService.AssingRoleEndpointAsync(request.Role, request.Menu, request.EndpointCode, request.type);
                return new();
            }
            finally
            {
                sw.Stop();
                _metricsService.ObserveHistogram("AssignRoleEndpoint_duration_seconds", sw.Elapsed.TotalSeconds);

            }
          
           
        }
    }
}
