using FinPay.Application.DTOs;

using FinPay.Application.DTOs.User;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IUserService
    {
       public Task<CreateUserResponse> Signup(SignupDto signup);
        public Task<TokenDto> Login(string email, string password, int accessTokenLifeTime, int refreshTokenLifeTime);
        public Task<TokenDto> RefreshTokenAsync(string accessToken, string refreshToken, int accessTokenLifeTime, int refreshTokenLifeTime);
        public Task<List<UserResponseDto>> GetAllUserAsync(int page,int size);
        public Task AssingRoleToUser(string id, string[] roles);
        public Task<bool> HasRolePermissionToEndpointAsync(string name,string code);
     


    }
}
