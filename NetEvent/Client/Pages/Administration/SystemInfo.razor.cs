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
    // private string searchString1 = "";

    // private HashSet<Element> selectedItems = new HashSet<Element>();

    // private IEnumerable<String> Elements = new List<String>();

    // protected override async Task OnInitializedAsync()
    // {
    //     Elements = await httpClient.GetFromJsonAsync<List<Element>>("webapi/periodictable");
    // }

    // private bool FilterFunc1(Element element) => FilterFunc(element, searchString1);

    // private bool FilterFunc(Element element, string searchString)
    // {
    //     if (string.IsNullOrWhiteSpace(searchString))
    //         return true;
    //     if (element.Sign.Contains(searchString, StringComparison.OrdinalIgnoreCase))
    //         return true;
    //     if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
    //         return true;
    //     if ($"{element.Number} {element.Position} {element.Molar}".Contains(searchString))
    //         return true;
    //     return false;
    // }

        #region Injects

        [Inject]
        private ISystemInfoDataService _SystemInfoDataService { get; set; } = default!;

        #endregion


        private IList<SystemInfoDto> _SystemInfos = new List<SystemInfoDto>();
        private SystemInfo _SystemInfo = new SystemInfo();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _SystemInfos = await _SystemInfoDataService.GetSystemInfoDataAsync(cts.Token);
        }



    }
}
