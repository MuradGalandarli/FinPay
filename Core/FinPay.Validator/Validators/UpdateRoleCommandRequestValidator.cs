using FinPay.Application.Features.Commands.AppRole.UpdateRole;
using FluentValidation;

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
