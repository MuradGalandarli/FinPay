using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinPay.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Validation xətası baş verdi.") { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception exception) : base(message, exception) { }

        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(FluentValidation.Results.ValidationResult result)
            : base("Validation failed.")
        {
            Errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

    }
}
