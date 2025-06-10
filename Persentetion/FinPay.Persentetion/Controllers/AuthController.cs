using FinPay.Application.DTOs;
using FinPay.Application.Features.Commands.AppUser.LoginUser;
using FinPay.Application.Features.Commands.AppUser.RefreshToken;
using FinPay.Application.Features.Commands.AppUser.SignupUser;
using FinPay.Application.Service.Authentications;
using FinPay.Domain;
using FinPay.Domain.Identity;
using FinPay.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinPay.Persentetion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupCommandRequest signupCommandRequest)
            {
            SignupCommandResponse signupCommandResponse = await _mediator.Send(signupCommandRequest);
            return Ok(signupCommandResponse);

        }
        [HttpPost("[action]")]
        //[HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
        LoginUserCommandResponse loginUserCommandResponse = await _mediator.Send(loginUserCommandRequest);
            return Ok(loginUserCommandResponse);
        }
        [AllowAnonymous]
        [HttpPost("token/refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommandRequest tokenModel)
        { 
            RefreshTokenCommandResponse refreshTokenCommandResponse =  await _mediator.Send(tokenModel);
            return Ok(refreshTokenCommandResponse);
        }

        }
}
