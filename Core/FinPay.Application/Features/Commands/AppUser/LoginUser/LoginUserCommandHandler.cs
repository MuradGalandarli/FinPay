using AutoMapper;
using FinPay.Application.DTOs;
using FinPay.Application.Service;
using MediatR;

namespace FinPay.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
    private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            TokenDto token = await _userService.Login(request.Username, request.Password ,100,200);

            return _mapper.Map<LoginUserCommandResponse>(token);
           
        }
    }
}
