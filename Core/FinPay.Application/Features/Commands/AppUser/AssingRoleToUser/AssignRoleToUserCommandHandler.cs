using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using System.Diagnostics;

namespace FinPay.Application.Features.Commands.AppUser.AssingRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommandRequest, AssignRoleToUserCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IValidator<AssignRoleToUserCommandRequest> _validator;
        private readonly IMetricsService _metricsService;
        public AssignRoleToUserCommandHandler(IUserService userService, IValidator<AssignRoleToUserCommandRequest> validator, IMetricsService metricsService = null)
        {
            _userService = userService;
            _validator = validator;
            _metricsService = metricsService;
        }

        public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("AssignRoleToUser");
            var sw = Stopwatch.StartNew();
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                await _userService.AssignRoleToUser(request.Id, request.Roles);
                return new();
            }
            finally
            {
                sw.Stop();
                _metricsService.ObserveHistogram("AssignRoleToUser_duration_seconds", sw.Elapsed.TotalSeconds);
            }
          
        }
    }
}
