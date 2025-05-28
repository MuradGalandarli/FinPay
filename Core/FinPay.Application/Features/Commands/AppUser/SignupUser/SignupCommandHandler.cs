
using FinPay.Application.DTOs.User;
using FinPay.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.SignupUser
{
    public class SignupCommandHandler : IRequestHandler<SignupCommandRequest, SignupCommandResponse>
    {
        private readonly IUserService _userService;

        public SignupCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<SignupCommandResponse> Handle(SignupCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse signupCommandResponse = await _userService.Signup(new() {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password, 
            
            });

          return new() {
          Message = signupCommandResponse.Message,
          Succeeded = signupCommandResponse.Succeeded,
          };
        }
    }
}
