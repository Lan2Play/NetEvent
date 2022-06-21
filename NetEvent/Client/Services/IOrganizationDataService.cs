using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface ISystemSettingsDataService
    {
        Task<List<SystemSettingValueDto>> GetOrganizationDataAsync(CancellationToken cancellationToken);
    }
}
