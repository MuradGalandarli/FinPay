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

        public async Task SendMail(string subject,string body)
        {
           
            SmtpClient client = new SmtpClient(_configuration["EmailSettings:Server"], int.Parse(_configuration.GetSection("EmailSettings:Port").Value));
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential( _configuration["EmailSettings:SenderEmail"], "ehwy nyxe tbys cacs");

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration["EmailSettings:SenderEmail"]);
            mailMessage.To.Add(_configuration["EmailSettings:SenderEmail"]);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append(body);
          
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}
