using FinPay.Application.DTOs;
using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, RefreshTokenCommandResponse>
    {
        private readonly IUserService _userService;

        public RefreshTokenCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            TokenDto token = await _userService.RefreshTokenAsync(request.AccessToken, request.RefreshToken,100,200);
            return new()
            {
                RefreshToken = token.RefreshToken,
                AccessToken = token.AccessToken,
            };

        }
    }
}


