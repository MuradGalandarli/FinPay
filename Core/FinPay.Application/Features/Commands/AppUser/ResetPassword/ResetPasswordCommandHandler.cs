using FinPay.Application.Service;
using MediatR;

namespace FinPay.Application.Features.Commands.AppUser.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, ResetPasswordCommandRespose>
    {
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResetPasswordCommandRespose> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
        {
           await _userService.PasswordResetAsync(request.Email);
            return new();
        }

      
    }
}
