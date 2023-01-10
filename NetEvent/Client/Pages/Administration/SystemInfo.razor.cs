using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class SystemInfo
    {
        #region Injects

        [Inject]
        private ISystemInfoDataService SystemInfoDataService { get; set; } = default!;

        #endregion

        private readonly IList<SystemInfoComponentEntryDto> _ClientComponents = new List<SystemInfoComponentEntryDto>();
        private SystemInfoDto _SystemInfos = new();
        private string searchStringComponents = string.Empty;
        private string searchStringClientComponents = string.Empty;
        private string searchStringVersions = string.Empty;
        private string searchStringHealth = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _SystemInfos = await SystemInfoDataService.GetSystemInfoDataAsync(cts.Token);

            var currentDomain = AppDomain.CurrentDomain;
            var assems = currentDomain.GetAssemblies();
            foreach (var assem in assems)
            {
                _ClientComponents.Add(new SystemInfoComponentEntryDto(assem.ManifestModule.Name, assem.ToString()));
            }
        }

        private bool FilterFuncComponents(SystemInfoComponentEntryDto systeminfocomponententry) => InternalFilterFuncComponents(systeminfocomponententry, searchStringComponents);

        private bool FilterFuncComponentsClient(SystemInfoComponentEntryDto systeminfocomponententry) => InternalFilterFuncComponents(systeminfocomponententry, searchStringClientComponents);

        private static bool InternalFilterFuncComponents(SystemInfoComponentEntryDto systeminfocomponententry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return true;
            }

            if (systeminfocomponententry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (systeminfocomponententry.Version.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private bool FilterFuncVersions1(SystemInfoVersionEntryDto systeminfoversionentry) => FilterFuncVersions(systeminfoversionentry, searchStringVersions);

        private static bool FilterFuncVersions(SystemInfoVersionEntryDto systeminfoversionentry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return true;
            }

            if (systeminfoversionentry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(systeminfoversionentry.Version) && systeminfoversionentry.Version.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private bool FilterFuncHealth1(SystemInfoHealthEntryDto systeminfohealthentry) => FilterFuncHealth(systeminfohealthentry, searchStringHealth);

        private static bool FilterFuncHealth(SystemInfoHealthEntryDto systeminfohealthentry, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return true;
            }

            if (systeminfohealthentry.Component.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (systeminfohealthentry.Value.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
