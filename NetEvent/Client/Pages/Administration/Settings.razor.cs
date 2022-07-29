using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
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

        private readonly SystemSettings _SystemSettings = SystemSettings.Instance;
        private bool _Loading = true;
        private IList<SystemSettingValueDto> _OrganizationData = new List<SystemSettingValueDto>();
        private IList<SystemSettingValueDto> _AuthenticationData = new List<SystemSettingValueDto>();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _OrganizationData = await _SystemSettingsDataService.GetSystemSettingsAsync(SystemSettingGroup.OrganizationData, cts.Token);
            _AuthenticationData = await _SystemSettingsDataService.GetSystemSettingsAsync(SystemSettingGroup.AuthenticationData, cts.Token);

            _Loading = false;
        }

        private SystemSettingValueDto? GetValue(string key)
        {
            return _OrganizationData.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal))
            ?? _AuthenticationData.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal));
        }
    }
}
