using MediatR;

namespace FinPay.Application.Features.Commands.AppUser.ResetPassword
{
    public class ResetPasswordCommandRequest:IRequest<ResetPasswordCommandRespose>
    {
        public string Email { get; set; }
    }
}