using FinPay.Application.DTOs;
using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, RefreshTokenCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IMetricsService _metricsService;

        public RefreshTokenCommandHandler(IUserService userService, IMetricsService metricsService)
        {
            _userService = userService;
            _metricsService = metricsService;
        }

        public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("RefreshToken");
            var sw = Stopwatch.StartNew();

            TokenDto token = await _userService.RefreshTokenAsync(request.AccessToken, request.RefreshToken, 100, 200);
            sw.Stop();
            _metricsService.ObserveHistogram("RefreshToken_duration_seconds", sw.Elapsed.TotalSeconds);

            return new()
            {
                RefreshToken = token.RefreshToken,
                AccessToken = token.AccessToken,
            };

        }
    }
}


