using System.Threading;
using System.Threading.Tasks;

namespace NetEvent.Server.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipient, string subject, string content, CancellationToken cancellationToken);
    }
}
