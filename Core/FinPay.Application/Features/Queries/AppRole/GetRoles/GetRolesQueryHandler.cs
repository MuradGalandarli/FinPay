using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.AppRole.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRoleQueryResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IMetricsService _metricsService;

        public GetRolesQueryHandler(IRoleService roleService, IMetricsService metricsService)
        {
            _roleService = roleService;
            _metricsService = metricsService;
        }

        public async Task<GetRoleQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("GetAllRols");
            var sw = Stopwatch.StartNew();
           var data = await _roleService.GetAllRols(request.Size,request.Page);
            sw.Stop();
            _metricsService.ObserveHistogram("GetAllRoles_duration_seconds", sw.Elapsed.TotalSeconds);
                    
            return new()
            {
              Roles = data,
            };
        }
    }
}
