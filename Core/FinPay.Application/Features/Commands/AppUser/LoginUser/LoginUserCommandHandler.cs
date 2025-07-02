using AutoMapper;
using FinPay.Application.DTOs;
using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using System.Diagnostics;

namespace FinPay.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginUserCommandRequest> _validator;
        private readonly IMetricsService _metricsService;

        public LoginUserCommandHandler(IUserService userService, IMapper mapper, IValidator<LoginUserCommandRequest> validator, IMetricsService metricsService = null)
        {
            _userService = userService;
            _mapper = mapper;
            _validator = validator;
            _metricsService = metricsService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("Login");
            var sw = Stopwatch.StartNew();
            try
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                    throw new Exceptions.ValidationException(validationResult);

                TokenDto token = await _userService.Login(request.Username, request.Password, 100, 200);

                return _mapper.Map<LoginUserCommandResponse>(token);

            }
            finally
            {
                sw.Stop();
                _metricsService.ObserveHistogram("Login_duration_seconds", sw.Elapsed.TotalSeconds);
            }


        }
    }
}
