using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IOrganizationDataService
    {
        Task<List<OrganizationDataDto>> GetOrganizationDataAsync(CancellationToken cancellationToken);
    }
}