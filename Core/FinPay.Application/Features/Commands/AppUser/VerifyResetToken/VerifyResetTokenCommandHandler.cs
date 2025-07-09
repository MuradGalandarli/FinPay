using FinPay.Application.Repositoryes.UserAccount;
using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.VerifyResetToken
{
    public class VerifyResetTokenCommandHandler : IRequestHandler<VerifyResetTokenCommandRequest, VerifyResetTokenCommandResponse>
    {
        private readonly IUserService _userService;

        public VerifyResetTokenCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<VerifyResetTokenCommandResponse> Handle(VerifyResetTokenCommandRequest request, CancellationToken cancellationToken)
        {
          var status = await _userService.VerifyResetTokenAsync(request.ResetToken, request.UserId);
            return new()
            {
                Status = status
            };
                }
    }
}
