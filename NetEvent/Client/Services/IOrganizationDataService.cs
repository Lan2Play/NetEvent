using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IOrganizationDataService
    {
        Task<List<OrganizationDataDto>> GetOrganizationDataAsync(CancellationToken cancellationToken);
    }
}
