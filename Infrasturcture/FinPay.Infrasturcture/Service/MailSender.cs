using FinPay.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FinPay._Infrastructure.Service
{
    public class MailSender : IMailSender
    {
        private readonly IConfiguration _configuration;

        public MailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMail(string to,string subject,string body)
        {
           
            SmtpClient client = new SmtpClient(_configuration["EmailSettings:Server"], int.Parse(_configuration.GetSection("EmailSettings:Port").Value));
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential( _configuration["EmailSettings:SenderEmail"], "ehwy nyxe tbys cacs");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration["EmailSettings:SenderEmail"]);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append(body);
          
            mailMessage.Body = mailBody.ToString();

            client.Send(mailMessage);
        }

        public async Task SendPasswordResetMail(string to,string userId,string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Salam<br> Eger Yeni sifre teleb etmisinizse asagidaki likden deyise bilersiz.<br><strong>" +
            "a< target = \"_blank\" href =\"..../");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\"> Yeni sifre telebi ucun tiklayin </a> </strong>");
            await SendMail(to, "Sifre yenileme", mail.ToString());
        }


    }
}
