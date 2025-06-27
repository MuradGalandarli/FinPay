using AutoMapper;
using FinPay.Application.DTOs;
using FinPay.Application.Service.Payment;
using MediatR;


namespace FinPay.Application.Features.Queries.Transaction.PaymrntTransaction
{
    public class PaymrntTransactionQueryHandler : IRequestHandler<PaymrntTransactionQueryRequest, List<PaymrntTransactionQueryRespose>>
    {
        private readonly IPaymentTransaction _paymentTransaction;
        private readonly IMapper _mapper;
        public PaymrntTransactionQueryHandler(IPaymentTransaction paymentTransaction, IMapper mapper)
        {
            _paymentTransaction = paymentTransaction;
            _mapper = mapper;
        }

        public async Task<List<PaymrntTransactionQueryRespose>> Handle(PaymrntTransactionQueryRequest request, CancellationToken cancellationToken)
        {
           var transaction = await _paymentTransaction.GetTransactionsByUserAccountId(request.UserAccountId);

            return _mapper.Map<List<PaymrntTransactionQueryRespose>>(transaction);
            
        }
    }
}
