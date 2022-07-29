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
using MudBlazor;
using NetEvent.Client;
using NetEvent.Client.Shared;
using NetEvent.Client.Components;
using NetEvent.Shared.Config;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;
using System.Threading;

namespace NetEvent.Client.Components.Administration
{
    public partial class SystemSettingControl
    {
        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        [Parameter]
        public SystemSetting SystemSetting { get; set; } = default!;

        [Parameter]
        public SystemSettingValueDto? Value { get; set; } = default!;

        private async Task OnSettingsValueChanged(SystemSetting setting, object? value)
        {
            if (value is null)
            {
                return;
            }

            var result = await _SystemSettingsDataService.UpdateSystemSetting(setting.SettingType, new SystemSettingValueDto(setting.Key, value.ToString() ?? string.Empty), CancellationToken.None);
            if (result.Successful && Value != null)
            {
                Value.Value = value.ToString() ?? string.Empty;

            }
        }
    }
}
