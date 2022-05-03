using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

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
