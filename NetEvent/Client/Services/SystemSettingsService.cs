using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
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

                var result = await client.GetFromJsonAsync<List<SystemSettingValueDto>>($"api/system/settings/{systemSettingGroup}/all", cancellationToken);

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

                var response = await client.PostAsJsonAsync($"api/system/settings/{systemSettingGroup}", systemSetting, cancellationToken);

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

        public async Task<List<SystemImageWithUsagesDto>> GetSystemImagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var result = await client.GetFromJsonAsync<List<SystemImageWithUsagesDto>>("/api/system/image/all", cancellationToken);

                if (result == null)
                {
                    _Logger.LogError("Unable to get images data from backend");
                    return new List<SystemImageWithUsagesDto>();
                }

                return result;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get images data from backend");
                return new List<SystemImageWithUsagesDto>();
            }
        }

        public async Task<ServiceResult<string>> UploadSystemImage(IBrowserFile file, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                using var formData = new MultipartFormDataContent();
                var streamContent = new StreamContent(file.OpenReadStream(cancellationToken: cancellationToken));
                var imageName = Path.GetFileNameWithoutExtension(file.Name);
                formData.Add(streamContent, imageName, Path.GetFileName(file.Name));

                var response = await client.PostAsync($"api/system/image/{imageName}", formData, cancellationToken);

                response.EnsureSuccessStatusCode();
                var uploadedImageId = await response.Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
                if (!string.IsNullOrEmpty(uploadedImageId))
                {
                    return ServiceResult<string>.Success(uploadedImageId, "SystemSettingService.UploadSystemImage.Success");
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to upload image in backend.");
            }

            return ServiceResult<string>.Error("SystemSettingService.UploadSystemImage.Error");
        }

        public async Task<ServiceResult> DeleteSystemImage(string imageId, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.DeleteAsync($"api/system/image/{imageId}", cancellationToken);

                response.EnsureSuccessStatusCode();
                return ServiceResult.Success("SystemSettingService.DeleteImageAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to delete image {imageId}.", imageId);
            }

            return ServiceResult<string>.Error("SystemSettingService.DeleteImageAsync.Error");
        }

        private static string GetCallbackKey(SystemSettingGroup systemSettingGroup, string key)
        {
            return $"{systemSettingGroup}.{key}";
        }
    }
}
