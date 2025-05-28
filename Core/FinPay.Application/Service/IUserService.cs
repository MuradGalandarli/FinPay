using FinPay.Application.DTOs;
using FinPay.Application.DTOs.User;
using FinPay.Application.Features.Commands.AppUser.RefreshToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IUserService
    {
       public Task<CreateUserResponse> Signup(SignupDto signup);
        public Task<TokenDto> Login(string email, string password, int accessTokenLifeTime, int refreshTokenLifeTime);
        public Task<TokenDto> RefreshTokenAsync(string accessToken, string refreshToken, int accessTokenLifeTime, int refreshTokenLifeTime);
    }
}
