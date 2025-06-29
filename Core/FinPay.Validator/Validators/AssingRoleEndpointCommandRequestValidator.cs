using FinPay.Application.Features.Commands.AuthorizationEndpoint.AssingRoleEndpoint;
using FinPay.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator.Validators
{
    public class AssingRoleEndpointCommandRequestValidator:AbstractValidator<AssingRoleEndpointCommandRequest>
    {
        public AssingRoleEndpointCommandRequestValidator()
        {
            RuleFor(x => x.Menu).MaximumLength(3);
        }
    }
}
