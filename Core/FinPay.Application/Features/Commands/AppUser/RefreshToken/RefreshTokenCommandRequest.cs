using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinPay.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenCommandRequest:IRequest<RefreshTokenCommandResponse>
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}