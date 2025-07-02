using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.Payment.UserAccount
{
    public class UserAccountCommandHandler : IRequestHandler<UserAccountCommandRequest, UserAccountCommandResponse>
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IMetricsService _metricsService;

        public UserAccountCommandHandler(IUserAccountService userAccountService, IMetricsService metricsService = null)
        {
            _userAccountService = userAccountService;
            _metricsService = metricsService;
        }

        public async Task<UserAccountCommandResponse> Handle(UserAccountCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("CreateUserAccount");
            var sw = Stopwatch.StartNew();

            var response = await _userAccountService.CreateUserAccount(new() { UserId = request.UserId,LinkedPaypalEmail = request.LinkedPaypalEmail});
            sw.Stop();
            _metricsService.ObserveHistogram("CreateUserAccount_duration_seconds", sw.Elapsed.TotalSeconds);
            return new()
            {
                Status = response
            };
        }
    }
}
