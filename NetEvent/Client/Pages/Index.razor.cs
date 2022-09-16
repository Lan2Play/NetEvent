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
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using System.Threading;

namespace NetEvent.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        private string? _OrganizationName;
        private string? _OrganizationAboutUs;

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            _OrganizationName = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.OrganizationName, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
            _OrganizationAboutUs = (await _SystemSettingsDataService.GetSystemSettingAsync(SystemSettingGroup.OrganizationData, SystemSettings.OrganizationData.AboutUs, cancellationTokenSource.Token).ConfigureAwait(false))?.Value;
        }
    }
}
