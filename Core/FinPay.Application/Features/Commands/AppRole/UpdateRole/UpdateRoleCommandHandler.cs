using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppRole.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<UpdateRoleCommandRequest> _validator;
        private readonly IMetricsService _metricsService;

        public UpdateRoleCommandHandler(IRoleService roleService, IValidator<UpdateRoleCommandRequest> validator, IMetricsService metricsService = null)
        {
            _roleService = roleService;
            _validator = validator;
            _metricsService = metricsService;
        }

        public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("UpdateRole");
            var sw = Stopwatch.StartNew();
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                bool status = await _roleService.UpdateRole(request.Id, request.Name);
                return new()
                {
                    Status = status,
                };
            }
            finally 
            {
                sw.Stop();
                _metricsService.ObserveHistogram("UpdateRole_duration_seconds", sw.Elapsed.TotalSeconds);
            }
           
        }
    }
}
