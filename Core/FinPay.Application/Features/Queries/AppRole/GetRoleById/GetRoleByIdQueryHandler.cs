using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.AppRole.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IMetricsService _metricsService;

        public GetRoleByIdQueryHandler(IRoleService roleService, IMetricsService metricsService)
        {
            _roleService = roleService;
            _metricsService = metricsService;
        }

        public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("GetRoleById");
            var sw = Stopwatch.StartNew();
            var data = await _roleService.GetRoleById(request.id);
            sw.Stop();
            _metricsService.ObserveHistogram("GetRoleById_duration_seconds", sw.Elapsed.TotalSeconds);
            return new() { Id = data.id,Name = data.name };

        }
    }
}
