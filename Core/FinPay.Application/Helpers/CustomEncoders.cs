using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;


namespace FinPay.Application.Helpers
{
    public static class CustomEncoders
    {
        public static string UrlEncode(this string value)
        {
            byte[] tokenByts = Encoding.UTF8.GetBytes(value);
            string resetToken = WebEncoders.Base64UrlEncode(tokenByts);
            return resetToken;

        }
        public static string UrlDecode(this string value)
        {
            byte[] tokenBytes = WebEncoders.Base64UrlDecode(value);
            string resetToken = Encoding.UTF8.GetString(tokenBytes);
            return resetToken;
        }
    }
}
