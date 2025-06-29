using FinPay.Application.Features.Commands.Payment.PaymentTransaction;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator.Validators
{
    public class PaymentTransactionCommandRequestValidator:AbstractValidator<PaymentTransactionCommandRequest>
    {
        public PaymentTransactionCommandRequestValidator()
        {
            RuleFor(x => x.UserAccountId).NotEmpty().NotEmpty();
            RuleFor(x => x.Amount).NotEmpty().NotEmpty().Must(x=>x > 0);
        }
    }
}
