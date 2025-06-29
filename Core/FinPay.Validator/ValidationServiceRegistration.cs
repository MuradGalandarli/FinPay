using FinPay.Validator.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Validator
{
    public static class ValidationServiceRegistration
    {
        public static void AddValidationService(this IServiceCollection sevices)
        {
            sevices.AddValidatorsFromAssemblyContaining(typeof(AssingRoleEndpointCommandRequestValidator));
        }
    }
}
