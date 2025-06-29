using FinPay.Application.Features.Commands.AppUser.SignupUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator.Validators
{
    public class SignupCommandRequestValidator:AbstractValidator<SignupCommandRequest>
    {
        public SignupCommandRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();

        }
    }
}
