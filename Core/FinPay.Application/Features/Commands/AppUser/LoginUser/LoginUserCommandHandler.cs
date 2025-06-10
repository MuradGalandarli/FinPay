using FinPay.Application.DTOs;
using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
    private readonly IUserService _userService;

        public LoginUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            TokenDto token = await _userService.Login(request.Username, request.Password ,100,200);

            return new()
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                RefreshTokenExpiresAt = token.RefreshTokenExpiresAt,
                
            };
        }
    }
}
