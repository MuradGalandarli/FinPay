using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Exceptions
{
    public class AuthenticationErrorException:Exception
    {
        public AuthenticationErrorException():base("Authentication error") {}

        public AuthenticationErrorException(string message):base(message) {}

        public AuthenticationErrorException(string message, Exception exception) : base(message,exception) { }
      

    }
}
