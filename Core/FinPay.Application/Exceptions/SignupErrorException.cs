using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Exceptions
{
    public class SignupErrorException:Exception
    {
        public SignupErrorException():base("Failed to create user") {}
        public SignupErrorException(string message):base(message){}
        public SignupErrorException(string message,Exception exception):base(message,exception){}
       
    }
}
