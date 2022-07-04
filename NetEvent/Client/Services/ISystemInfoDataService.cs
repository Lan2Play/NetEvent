using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface ISystemInfoDataService
    {
        Task<SystemInfoDto> GetSystemInfoDataAsync(CancellationToken cancellationToken);
    }
}
