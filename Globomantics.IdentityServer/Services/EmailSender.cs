using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Globomantics.IdentityServer.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromAddress;

        public EmailSender(IConfiguration config)
        {
            _smtpServer = config.GetValue<string>("Email:SmtpServer");
            _smtpPort = config.GetValue<int>("Email:SmtpPort");
            _fromAddress = config.GetValue<string>("Email:SenderAddress");
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(email));

            using var client = new SmtpClient(_smtpServer, _smtpPort);
            await client.SendMailAsync(message);
        }
    }
}
