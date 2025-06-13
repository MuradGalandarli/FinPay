using MediatR;

namespace FinPay.Application.Features.Commands.Payment.UserAccount
{
    public class UserAccountCommandRequest:IRequest<UserAccountCommandResponse>
    {
        public string UserId { get; set; }
        public string? LinkedPaypalEmail { get; set; }
    }
}