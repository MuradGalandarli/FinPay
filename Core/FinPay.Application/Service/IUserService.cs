using FinPay.Application.DTOs;
using FinPay.Application.DTOs.User;

namespace FinPay.Application.Service
{
    public interface IUserService
    {
       public Task<CreateUserResponse> Signup(SignupDto signup);
        public Task<TokenDto> Login(string email, string password, int accessTokenLifeTime, int refreshTokenLifeTime);
        public Task<TokenDto> RefreshTokenAsync(string accessToken, string refreshToken, int accessTokenLifeTime, int refreshTokenLifeTime);
        public Task<List<UserResponseDto>> GetAllUserAsync(int page,int size);
        public Task AssignRoleToUser(string id, string[] roles);
        public Task<bool> HasRolePermissionToEndpointAsync(string name,string code);
        public Task PasswordResetAsync(string email);
        public Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
        public Task UpdatePassword(string userId, string resetToken, string newPassword);


    }
}
