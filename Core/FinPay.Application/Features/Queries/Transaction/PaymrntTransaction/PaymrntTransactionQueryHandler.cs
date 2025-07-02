using AutoMapper;
using FinPay.Application.DTOs;
using FinPay.Application.Service;
using FinPay.Application.Service.Payment;
using MediatR;
using System.Diagnostics;


namespace FinPay.Application.Features.Queries.Transaction.PaymrntTransaction
{
    public class PaymrntTransactionQueryHandler : IRequestHandler<PaymrntTransactionQueryRequest, List<PaymrntTransactionQueryRespose>>
    {
        private readonly IPaymentTransaction _paymentTransaction;
        private readonly IMapper _mapper;
        private readonly IMetricsService _metricsService;
        public PaymrntTransactionQueryHandler(IPaymentTransaction paymentTransaction, IMapper mapper, IMetricsService metricsService)
        {
            _paymentTransaction = paymentTransaction;
            _mapper = mapper;
            _metricsService = metricsService;
        }

        public async Task<List<PaymrntTransactionQueryRespose>> Handle(PaymrntTransactionQueryRequest request, CancellationToken cancellationToken)
        {
            _metricsService.IncrementCounter("GetTransactionsByUserAccountId");
            var sw = Stopwatch.StartNew();
           var transaction = await _paymentTransaction.GetTransactionsByUserAccountId(request.UserAccountId);
            sw.Stop();
            _metricsService.ObserveHistogram("GetTransactionsByUserAccountId_duration_seconds", sw.Elapsed.TotalSeconds);
            return _mapper.Map<List<PaymrntTransactionQueryRespose>>(transaction);
            
        }
    }
}
