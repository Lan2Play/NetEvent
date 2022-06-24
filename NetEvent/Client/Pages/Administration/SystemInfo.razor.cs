using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

namespace NetEvent.Client.Pages.Administration
{
    public partial class SystemInfo
    {

        #region Injects

        [Inject]
        private ISystemInfoDataService _SystemInfoDataService { get; set; } = default!;

        #endregion
        private IList<SystemInfoDto> _SystemInfos = new List<SystemInfoDto>();
        private string searchString1 = "";


        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _SystemInfos = await _SystemInfoDataService.GetSystemInfoDataAsync(cts.Token);
        }


        private bool FilterFunc1(SystemInfoDto systeminfo) => FilterFunc(systeminfo, searchString1);

        private bool FilterFunc(SystemInfoDto systeminfo, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (systeminfo.Key.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (systeminfo.Value.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

    }
}
