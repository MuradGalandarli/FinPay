using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Features.Commands.AppUser.SignupUser
{
    public class SignupCommandResponse
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }
}
