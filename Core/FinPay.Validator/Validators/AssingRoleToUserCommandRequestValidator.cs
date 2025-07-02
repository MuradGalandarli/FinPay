using FinPay.Application.Features.Commands.AppUser.AssingRoleToUser;
using FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator.Validators
{
    public class AssingRoleToUserCommandRequestValidator:AbstractValidator<AssignRoleToUserCommandRequest>
    {
        public AssingRoleToUserCommandRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Roles).NotEmpty().NotNull();

        }
    }
}
