using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Application.Service
{
    public interface IMailSender
    {
        public Task SendMail(string to,string subject,string body);
        public Task SendPasswordResetMail(string to,string userId, string resetToken);
    }
}
