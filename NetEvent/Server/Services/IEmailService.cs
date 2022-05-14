using System.Threading;
using System.Threading.Tasks;
using NetEvent.Server.Models;

namespace NetEvent.Server.Services
{
    public interface IEmailService
    {
        Task SendMailAsync(string templateId, string receiverEmailAddress, NetEventEmailRenderModel subjectModel, NetEventEmailRenderModel templateModel, CancellationToken cancellationToken);
    }
}
