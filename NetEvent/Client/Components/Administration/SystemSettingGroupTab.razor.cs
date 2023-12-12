using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Components.Administration
{
    public partial class SystemSettingGroupTab
    {
        #region Injects

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private ISystemSettingsDataService SystemSettingsDataService { get; set; } = default!;

        #endregion

        #region Parameters

        [Parameter]
        public ISettingsGroup SettingsGroup { get; set; } = default!;

        #endregion

        private IList<SystemSettingValueDto> _Data = new List<SystemSettingValueDto>();
        private bool _Loading = true;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _Data = await SystemSettingsDataService.GetSystemSettingsAsync(SettingsGroup.SettingGroup, cts.Token);
            _Loading = false;
        }

        private SystemSettingValueDto? GetValue(string key)
        {
            return _Data.FirstOrDefault(x => x.Key.Equals(key, StringComparison.Ordinal))
                ?? new SystemSettingValueDto(key, string.Empty);
        }
    }
}
