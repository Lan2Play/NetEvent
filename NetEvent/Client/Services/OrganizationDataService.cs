using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public class SystemSettingsDataService : ISystemSettingsDataService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<SystemSettingsDataService> _Logger;

        public SystemSettingsDataService(IHttpClientFactory httpClientFactory, ILogger<SystemSettingsDataService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<List<SystemSettingValueDto>> GetOrganizationDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var result = await client.GetFromJsonAsync<List<SystemSettingValueDto>>($"api/system/{SystemSettingGroup.OrganizationData}/all", cancellationToken);

                if (result == null)
                {
                    _Logger.LogError("Unable to get organization data from backend");
                    return new List<SystemSettingValueDto>();
                }

                return result;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get organization data from backend");
                return new List<SystemSettingValueDto>();
            }
        }
    }
}
