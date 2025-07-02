using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.Payment.CaptureOrder
{
    public class CaptureOrderCommandHandler : IRequestHandler<CaptureOrderCommandRequest, CaptureOrderCommandResponse>
    {
        private readonly IPaymentTransaction _paymentTransaction;
        private readonly IMetricsService _metricsService;

        public CaptureOrderCommandHandler(IPaymentTransaction paymentTransaction, IMetricsService metricsService)
        {
            _paymentTransaction = paymentTransaction;
            _metricsService = metricsService;
        }

        public async Task<CaptureOrderCommandResponse> Handle(CaptureOrderCommandRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("CaptureOrder");
            var sw = Stopwatch.StartNew();

            string response = await _paymentTransaction.CaptureOrderAsync(request.OrderId,request.UserAccountId);
            sw.Stop();
            _metricsService.ObserveHistogram("CaptureOrder_duration_seconds", sw.Elapsed.TotalSeconds);
            return new()
            {
                Status = response,
            };
        }
    }
}
