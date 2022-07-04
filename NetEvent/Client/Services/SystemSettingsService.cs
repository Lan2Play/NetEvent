using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
    public class SystemSettingsService : ISystemSettingsDataService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<SystemSettingsService> _Logger;

        public SystemSettingsService(IHttpClientFactory httpClientFactory, ILogger<SystemSettingsService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<SystemSettingValueDto?> GetSystemSettingAsync(SystemSettingGroup systemSettingGroup, string key, CancellationToken cancellationToken)
        {
            var systemSettings = await GetSystemSettingsAsync(systemSettingGroup, cancellationToken);
            return systemSettings.FirstOrDefault(x => x.Key.Equals(SystemSettings.OrganizationName, StringComparison.Ordinal));
        }

        public async Task<List<SystemSettingValueDto>> GetSystemSettingsAsync(SystemSettingGroup systemSettingGroup, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var result = await client.GetFromJsonAsync<List<SystemSettingValueDto>>($"api/system/{systemSettingGroup}/all", cancellationToken);

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

        public async Task<ServiceResult> UpdateSystemSetting(SystemSettingGroup systemSettingGroup, SystemSettingValueDto systemSetting, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/system/{systemSettingGroup}", systemSetting, cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("RoleService.UpdateRoleAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return ServiceResult.Error("RoleService.UpdateRoleAsync.Error");
        }
    }
}
