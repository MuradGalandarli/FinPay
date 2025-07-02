using FinPay.Application.DTOs;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.GetCardBalanceByUserId
{
    public class GetCardBalanceByUserIdQueryHandler : IRequestHandler<GetCardBalanceByUserIdQueryRequest, GetCardBalanceByUserIdQueryResponse>
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IMetricsService _metricsService;

        public GetCardBalanceByUserIdQueryHandler(IUserAccountService userAccountService, IMetricsService metricsService)
        {
            _userAccountService = userAccountService;
            _metricsService = metricsService;
        }

        public async Task<GetCardBalanceByUserIdQueryResponse> Handle(GetCardBalanceByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("GetCardBalanceByAccountId");
            var sw = Stopwatch.StartNew();

            GetUserBalanceResponse getUserBalanceResponse = await _userAccountService.GetCardBalanceByAccountId(request.UserAccountId);
            sw.Stop();
            _metricsService.ObserveHistogram("GetCardBalanceByAccountId_duration_seconds", sw.Elapsed.TotalSeconds);

            return new()
            {
                Balance = getUserBalanceResponse.Balance,
                PaypalEmail = getUserBalanceResponse.PaypalEmail,

            };

        }
    }
}
