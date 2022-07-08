using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface ISystemSettingsDataService
    {
        Task<List<SystemSettingValueDto>> GetSystemSettingsAsync(SystemSettingGroup systemSettingGroup, CancellationToken cancellationToken);

        Task<SystemSettingValueDto?> GetSystemSettingAsync(SystemSettingGroup systemSettingGroup, string key, CancellationToken cancellationToken);

        Task<SystemSettingValueDto?> GetSystemSettingAsync(SystemSettingGroup systemSettingGroup, string key, Action<SystemSettingValueDto> valueChanged, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateSystemSetting(SystemSettingGroup systemSettingGroup, SystemSettingValueDto systemSetting, CancellationToken cancellationToken);

        Task<ServiceResult<string>> UploadSystemImage(IBrowserFile file, CancellationToken cancellationToken);
    }
}
