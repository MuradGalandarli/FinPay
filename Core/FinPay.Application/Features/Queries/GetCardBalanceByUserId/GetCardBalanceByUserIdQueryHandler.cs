using FinPay.Application.DTOs;
using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Queries.GetCardBalanceByUserId
{
    public class GetCardBalanceByUserIdQueryHandler : IRequestHandler<GetCardBalanceByUserIdQueryRequest, GetCardBalanceByUserIdQueryResponse>
    {
        private readonly IUserAccountService _userAccountService;

        public GetCardBalanceByUserIdQueryHandler(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task<GetCardBalanceByUserIdQueryResponse> Handle(GetCardBalanceByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            GetUserBalanceResponse getUserBalanceResponse = await _userAccountService.GetCardBalanceByAccountId(request.UserAccountId);
            return new() {
                Balance = getUserBalanceResponse.Balance,
            PaypalEmail = getUserBalanceResponse.PaypalEmail,
            
            };

        }
    }
}
