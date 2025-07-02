using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppRole.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IMetricsService _metricsService;

        public DeleteRoleCommandHandler(IRoleService roleService, IMetricsService metricsService = null)
        {
            _roleService = roleService;
            _metricsService = metricsService;
        }

        public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("DeleteRole");
            var sw = Stopwatch.StartNew();
            bool status = await _roleService.DeleteRole(request.Name);
            sw.Stop();
            _metricsService.ObserveHistogram("DeleteRole_duration_seconds", sw.Elapsed.TotalSeconds);
            return new() { Status = status};

        }
    }
}
