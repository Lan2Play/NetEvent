using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetEvent.Server.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridConfig _SendGridConfiguration;
        private readonly ILogger<SendGridEmailSender> _Logger;

        public SendGridEmailSender(SendGridConfig sendGridConfiguration, ILogger<SendGridEmailSender> logger)
        {
            _SendGridConfiguration = sendGridConfiguration;
            _Logger = logger;
        }

        public async Task SendEmailAsync(string recipient, string subject, string content, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_SendGridConfiguration.SendGridKey))
            {
                _Logger.LogError("SendGridKey is not set");
                return;
            }

            var client = new SendGridClient(_SendGridConfiguration.SendGridKey);

            var msg = new SendGridMessage
            {
                From = new EmailAddress(_SendGridConfiguration.EmailSenderAddress),
                Subject = subject,
                PlainTextContent = content,
                HtmlContent = content
            };

            msg.AddTo(new EmailAddress(recipient));

            msg.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(msg, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _Logger.LogInformation("Email to {recipient} queued successfully!", recipient);
            }
            else
            {
                _Logger.LogInformation("Failure Email to {recipient}", recipient);
            }
        }
    }
}
