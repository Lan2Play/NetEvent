using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using NetEvent.Client;
using NetEvent.Client.Shared;
using MudBlazor;
using Microsoft.AspNetCore.Authorization;
using NetEvent.Client.Services;
using System.Threading;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Config;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Settings
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        #endregion

        private bool _Loading = true;
        private IList<SystemSettingValueDto> _OrganizationData = new List<SystemSettingValueDto>();
        private SystemSettings _SystemSettings = new SystemSettings();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _OrganizationData = await _SystemSettingsDataService.GetOrganizationDataAsync(cts.Token);
            _Loading = false;
        }

        private void OnSettingsValueChanged(SystemSetting setting, object? value)
        {
            //await _SystemSettingsDataService.UpdateSettingAsync(setting, value);
        }
    }
}
