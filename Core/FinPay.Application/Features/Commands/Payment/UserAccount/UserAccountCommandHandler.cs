using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Application.Service.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.Payment.UserAccount
{
    public class UserAccountCommandHandler : IRequestHandler<UserAccountCommandRequest, UserAccountCommandResponse>
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountCommandHandler(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task<UserAccountCommandResponse> Handle(UserAccountCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _userAccountService.CreateUserAccount(new() { UserId = request.UserId,LinkedPaypalEmail = request.LinkedPaypalEmail});
            return new()
            {
                Status = response
            };
        }
    }
}
