using FinPay.Application.Features.Commands.AppRole.UpdateRole;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator.Validators
{
    public class UpdateRoleCommandRequestValidator:AbstractValidator<UpdateRoleCommandRequest>
    {
        public UpdateRoleCommandRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
