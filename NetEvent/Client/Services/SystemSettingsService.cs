using System;
using System.Collections.Concurrent;
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
        private readonly ILogger<SystemSettingsService> _Logger;
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ConcurrentDictionary<string, ConcurrentBag<Action<SystemSettingValueDto>>> _Callbacks = new ConcurrentDictionary<string, ConcurrentBag<Action<SystemSettingValueDto>>>();

        public SystemSettingsService(IHttpClientFactory httpClientFactory, ILogger<SystemSettingsService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<SystemSettingValueDto?> GetSystemSettingAsync(SystemSettingGroup systemSettingGroup, string key, CancellationToken cancellationToken)
        {
            var systemSettings = await GetSystemSettingsAsync(systemSettingGroup, cancellationToken);
            return systemSettings.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal));
        }

        public Task<SystemSettingValueDto?> GetSystemSettingAsync(SystemSettingGroup systemSettingGroup, string key, Action<SystemSettingValueDto> valueChanged, CancellationToken cancellationToken)
        {
            var bag = _Callbacks.GetOrAdd(GetCallbackKey(systemSettingGroup, key), s => new ConcurrentBag<Action<SystemSettingValueDto>>());
            bag.Add(valueChanged);
            return GetSystemSettingAsync(systemSettingGroup, key, cancellationToken);
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

                if (_Callbacks.TryGetValue(GetCallbackKey(systemSettingGroup, systemSetting.Key), out var callbacks))
                {
                    foreach (var callback in callbacks)
                    {
                        callback(systemSetting);
                    }
                }

                return ServiceResult.Success("RoleService.UpdateRoleAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return ServiceResult.Error("RoleService.UpdateRoleAsync.Error");
        }

        private static string GetCallbackKey(SystemSettingGroup systemSettingGroup, string key)
        {
            return $"{systemSettingGroup}.{key}";
        }
    }
}
