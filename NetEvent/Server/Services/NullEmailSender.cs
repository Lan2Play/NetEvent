using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NetEvent.Server.Services
{
    public class NullEmailSender : IEmailSender
    {
        private readonly ILogger<NullEmailSender> _Logger;

        public NullEmailSender(ILogger<NullEmailSender> logger)
        {
            _Logger = logger;
        }

        public Task SendEmailAsync(string recipient, string subject, string content, CancellationToken cancellationToken)
        {
            _Logger.LogInformation("NullEmailSender was asked to send E-Mail to {Receiver} with {Subject} and {Message}", recipient, subject, content);

            return Task.CompletedTask;
        }
    }
}
