﻿using System;
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
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        #endregion

        private readonly SystemSettings _SystemSettings = SystemSettings.Instance;
        private bool _Loading = true;
        private IList<SystemSettingValueDto> _OrganizationData = new List<SystemSettingValueDto>();
        private IList<SystemSettingValueDto> _StyleData = new List<SystemSettingValueDto>();
        private IList<SystemSettingValueDto> _AuthenticationData = new List<SystemSettingValueDto>();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _OrganizationData = await SystemSettingsDataService.GetSystemSettingsAsync(SystemSettingGroup.OrganizationData, cts.Token);
            _StyleData = await SystemSettingsDataService.GetSystemSettingsAsync(SystemSettingGroup.StyleData, cts.Token);
            _AuthenticationData = await SystemSettingsDataService.GetSystemSettingsAsync(SystemSettingGroup.AuthenticationData, cts.Token);

            _Loading = false;
        }

        private SystemSettingValueDto? GetValue(string key)
        {
            return _OrganizationData.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal))
                ?? _StyleData.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal))
                ?? _AuthenticationData.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal))
                ?? new SystemSettingValueDto(key, string.Empty);
        }
    }
}
