using FinPay.Application.Service;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.AssingRoleToUser
{
    public class AssingRoleToUserCommandHandler : IRequestHandler<AssingRoleToUserCommandRequest, AssingRoleToUserCommandRespose>
    {
        private readonly IUserService _userService;
        private readonly IValidator<AssingRoleToUserCommandRequest> _validator;

        public AssingRoleToUserCommandHandler(IUserService userService, IValidator<AssingRoleToUserCommandRequest> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        public async Task<AssingRoleToUserCommandRespose> Handle(AssingRoleToUserCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult);

            await _userService.AssingRoleToUser(request.Id,request.Roles);
            return new();
        }
    }
}
