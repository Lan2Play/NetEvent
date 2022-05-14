using System.Threading;
using System.Threading.Tasks;
using NetEvent.Server.Models;

namespace NetEvent.Server.Services
{
    public interface IEmailRenderer
    {
        Task<string?> RenderAsync(string templatKey, string template, NetEventEmailRenderModel model, CancellationToken cancellationToken);
    }
}
