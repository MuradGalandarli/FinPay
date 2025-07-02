
using FinPay.Application.DTOs.User;
using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.SignupUser
{
    public class SignupCommandHandler : IRequestHandler<SignupCommandRequest, SignupCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IValidator<SignupCommandRequest>_validator;
        private readonly IMetricsService _metricsService;

        public SignupCommandHandler(IUserService userService, IValidator<SignupCommandRequest> validator = null, IMetricsService metricsService = null)
        {
            _userService = userService;
            _validator = validator;
            _metricsService = metricsService;
        }

        public async Task<SignupCommandResponse> Handle(SignupCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("Signup");
            var sw = Stopwatch.StartNew();
            try
            {
                
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                CreateUserResponse signupCommandResponse = await _userService.Signup(new()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,

                });

                return new()
                {
                    Message = signupCommandResponse.Message,
                    Succeeded = signupCommandResponse.Succeeded,
                };
            }
            finally
            {
              sw.Stop();
                _metricsService.ObserveHistogram("Signup_duration_seconds", sw.Elapsed.TotalSeconds);
            }
           
            }
          
    }
}
