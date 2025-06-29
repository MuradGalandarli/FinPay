
using FinPay.Application.DTOs.User;
using FinPay.Application.Service;
using FluentValidation;
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
        private readonly IValidator<SignupCommandRequest>_validator;

        public SignupCommandHandler(IUserService userService, IValidator<SignupCommandRequest> validator = null)
        {
            _userService = userService;
            _validator = validator;
        }

        public async Task<SignupCommandResponse> Handle(SignupCommandRequest request, CancellationToken cancellationToken)
        {
            if (_validator.Validate(request).IsValid)
            {
                CreateUserResponse signupCommandResponse = await _userService.Signup(new()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,

                });

                return new()
                {
                    Message = signupCommandResponse.Message,
                    Succeeded = signupCommandResponse.Succeeded,
                };
            }
            throw new Exceptions.ValidationException();

        }
    }
}
