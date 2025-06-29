using FinPay.Application.Features.Commands.CardToCardTransaction.CardTransaction;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator.Validators
{
    public class CardTransactionCommandRequestValidator:AbstractValidator<CardTransactionCommandRequest>
    {
        public CardTransactionCommandRequestValidator() 
        {
            RuleFor(x => x.ToUserId).NotEmpty().NotNull();
            RuleFor(x => x.FromUserId).NotEmpty().NotNull();
            RuleFor(x => x.FromPaypalEmail).NotEmpty().NotNull();
            RuleFor(x => x.ToPaypalEmail).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull().Must(x=>x > 0);
        }
    }
}
