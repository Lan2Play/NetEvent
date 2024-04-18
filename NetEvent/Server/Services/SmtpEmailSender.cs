using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Configuration;

namespace NetEvent.Server.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpConfig _SmtpConfig;
        private readonly ILogger<SmtpEmailSender> _Logger;

        public SmtpEmailSender(SmtpConfig smtpConfig, ILogger<SmtpEmailSender> logger)
        {
            _SmtpConfig = smtpConfig;
            _Logger = logger;
        }

        public async Task SendEmailAsync(string recipient, string subject, string content, CancellationToken cancellationToken)
        {
            using var smtpClient = new SmtpClient(_SmtpConfig.Host)
            {
                Port = _SmtpConfig.Port,
                Credentials = new NetworkCredential(_SmtpConfig.Username, _SmtpConfig.Password),
                EnableSsl = true,
            };

            var message = new MailMessage
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = true,
                From = new MailAddress(_SmtpConfig.EmailSenderAddress),
            };

            message.To.Add(recipient);

            try
            {
                await smtpClient.SendMailAsync(message, cancellationToken).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex, "Error sending mail to {Recipient}", recipient);
            }
        }
    }
}
