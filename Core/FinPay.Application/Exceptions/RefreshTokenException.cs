using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Exceptions
{
    public class RefreshTokenException : Exception
    {
        public RefreshTokenException() : base("Invalid refresh token") { }
        public RefreshTokenException(string message) : base(message) { }
        public RefreshTokenException(string message,Exception exception) : base(message,exception) { }

    
    }
}
