using MediatR;

namespace FinPay.Application.Features.UpdatePassword
{
    public class UpdatePasswordCommandRequest:IRequest<UpdatePasswordCommandResponse>
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}