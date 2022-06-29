using System;
using System.Collections.Generic;
using System.Reflection;
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
        private SystemInfoDto _SystemInfos = new SystemInfoDto();
        private IList<SystemInfoComponentEntryDto> _ClientComponents = new List<SystemInfoComponentEntryDto>();
        private string searchStringComponents = "";
        private string searchStringClientComponents = "";
        private string searchStringVersions = "";
        private string searchStringHealth = "";


        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _SystemInfos = await _SystemInfoDataService.GetSystemInfoDataAsync(cts.Token);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            foreach (Assembly assem in assems)
            {
                _ClientComponents.Add(new SystemInfoComponentEntryDto(assem.ManifestModule.Name.ToString(), assem.ToString()));
            }

        }


        private bool FilterFuncComponents1(SystemInfoComponentEntryDto systeminfocomponententry) => FilterFuncComponents(systeminfocomponententry, searchStringComponents);

        private bool FilterFuncComponents(SystemInfoComponentEntryDto systeminfocomponententry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (systeminfocomponententry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (systeminfocomponententry.Version.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private bool FilterFuncClientComponents1(SystemInfoComponentEntryDto systeminfocomponententry) => FilterFuncClientComponents(systeminfocomponententry, searchStringClientComponents);

        private bool FilterFuncClientComponents(SystemInfoComponentEntryDto systeminfocomponententry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (systeminfocomponententry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (systeminfocomponententry.Version.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private bool FilterFuncVersions1(SystemInfoVersionEntryDto systeminfoversionentry) => FilterFuncVersions(systeminfoversionentry, searchStringVersions);

        private bool FilterFuncVersions(SystemInfoVersionEntryDto systeminfoversionentry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (systeminfoversionentry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (systeminfoversionentry.Version.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private bool FilterFuncHealth1(SystemInfoHealthEntryDto systeminfohealthentry) => FilterFuncHealth(systeminfohealthentry, searchStringHealth);

        private bool FilterFuncHealth(SystemInfoHealthEntryDto systeminfohealthentry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (systeminfohealthentry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (systeminfohealthentry.Value.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

    }
}
