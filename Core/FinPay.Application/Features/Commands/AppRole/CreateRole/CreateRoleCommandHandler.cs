using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppRole.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IMetricsService _metricsService;

        public CreateRoleCommandHandler(IRoleService roleService, IMetricsService metricsService)
        {
            _roleService = roleService;
            _metricsService = metricsService;
        }

        public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("CreateRole");
            var sw = Stopwatch.StartNew();

            bool status = await _roleService.CreateRole(request.Name);

            sw.Stop();
            _metricsService.ObserveHistogram("CreateRole_duration_seconds", sw.Elapsed.TotalSeconds);
            return new() { Status = status };
        }
    }


}
