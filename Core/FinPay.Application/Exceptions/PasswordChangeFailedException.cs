using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Exceptions
{
    public class PasswordChangeFailedException:Exception
    {
        public PasswordChangeFailedException() : base("Sifre deyiserken xeta oldu")
        {
        }
        public PasswordChangeFailedException(string message):base(message) 
        {
            
        }
        public PasswordChangeFailedException(Exception exception, string message) :base(message,exception) 
        {
            
        }
    }
}
