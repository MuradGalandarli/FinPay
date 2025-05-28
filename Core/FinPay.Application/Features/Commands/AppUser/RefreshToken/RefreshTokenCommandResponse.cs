using System.ComponentModel.DataAnnotations;

namespace FinPay.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenCommandResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}