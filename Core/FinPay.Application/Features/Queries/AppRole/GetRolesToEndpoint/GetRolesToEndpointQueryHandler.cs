using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.AppRole.GetRolesToEndpoint
{
    public class GetRolesToEndpointQueryHandler:IRequestHandler<GetRolesToEndpointQueryRequest, GetRolesToEndpointQueryResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;
        private readonly IMetricsService _metricsService;

        public GetRolesToEndpointQueryHandler(IAuthorizationEndpointService authorizationEndpointService, IMetricsService metricsService)
        {
            _authorizationEndpointService = authorizationEndpointService;
            _metricsService = metricsService;
        }

        public async Task<GetRolesToEndpointQueryResponse> Handle(GetRolesToEndpointQueryRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("GetRolesToEndpoint");
            var sw = Stopwatch.StartNew();
          var getRolesToEndpointQueryResponse = await _authorizationEndpointService.GetRolesToEndpointAsync(request.Code, request.Menu);
            sw.Stop();
            _metricsService.ObserveHistogram("GetRolesToEndpoint_duration_seconds", sw.Elapsed.TotalSeconds);
            return new()
            {
                Name = getRolesToEndpointQueryResponse
            };
        }
    }
}
