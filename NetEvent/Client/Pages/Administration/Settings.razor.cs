using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Settings
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        #endregion

        private readonly SystemSettings _SystemSettings = new();
        private bool _Loading = true;
        private IList<SystemSettingValueDto> _OrganizationData = new List<SystemSettingValueDto>();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _OrganizationData = await _SystemSettingsDataService.GetSystemSettingsAsync(SystemSettingGroup.OrganizationData, cts.Token);
            _Loading = false;
        }

        private async Task OnSettingsValueChanged(SystemSetting setting, object? value)
        {
            if (value is null)
            {
                return;
            }

            var result = await _SystemSettingsDataService.UpdateSystemSetting(setting.SettingType, new SystemSettingValueDto(setting.Key, value.ToString() ?? string.Empty), CancellationToken.None);
            if (result.Successful)
            {
                var existingData = _OrganizationData.FirstOrDefault(x => x.Key.Equals(setting.Key, StringComparison.OrdinalIgnoreCase));
                if (existingData != null)
                {
                    existingData.Value = value.ToString() ?? string.Empty;
                }
            }
        }
    }
}
