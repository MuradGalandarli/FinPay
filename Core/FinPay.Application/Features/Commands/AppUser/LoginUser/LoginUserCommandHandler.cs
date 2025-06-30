using AutoMapper;
using FinPay.Application.DTOs;
using FinPay.Application.Service;
using FluentValidation;
using MediatR;

namespace FinPay.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginUserCommandRequest> _validator;

        public LoginUserCommandHandler(IUserService userService, IMapper mapper, IValidator<LoginUserCommandRequest> validator)
        {
            _userService = userService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult);

            TokenDto token = await _userService.Login(request.Username, request.Password, 100, 200);

                return _mapper.Map<LoginUserCommandResponse>(token);
          
        }
    }
}
