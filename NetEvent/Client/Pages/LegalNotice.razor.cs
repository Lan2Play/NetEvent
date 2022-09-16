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

namespace NetEvent.Client.Pages
{
    public partial class LegalNotice
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        #endregion

        private readonly SystemSettings _SystemSettings = SystemSettings.Instance;
        private bool _Loading = true;
        private string _LegalNotice = string.Empty;
        private string _PrivacyPolicy = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _LegalNotice = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.LegalNotice, cts.Token))?.Value ?? string.Empty;
            _PrivacyPolicy = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.PrivacyPolicy, cts.Token))?.Value ?? string.Empty;

            _Loading = false;
        }
    }
}
