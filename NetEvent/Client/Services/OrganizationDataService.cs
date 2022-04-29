using NetEvent.Shared.Dto;
using System.Net.Http.Json;

namespace NetEvent.Client.Services
{
    public class OrganizationDataService : IOrganizationDataService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<OrganizationDataService> _Logger;

        public OrganizationDataService(IHttpClientFactory httpClientFactory, ILogger<OrganizationDataService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<List<OrganizationDataDto>> GetOrganizationDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var result = await client.GetFromJsonAsync<List<OrganizationDataDto>>("api/organization/all", cancellationToken);

                if (result == null)
                {
                    _Logger.LogError("Unable to get organization data from backend");
                    return new List<OrganizationDataDto>();
                }

                return result;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get organization data from backend");
                return new List<OrganizationDataDto>();
            }
        }
    }
}
