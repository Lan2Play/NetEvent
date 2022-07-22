﻿using System;
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
using System.Threading;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages.Administration
{
    public partial class ImageAdministration
    {
        #region Injects

        [Inject]
        private ISystemSettingsDataService _SystemSettingsDataService { get; set; } = default!;

        #endregion

        private const string ImageUrl = "/api/system/image/";
        private List<SystemImageDto> Images;

        protected async override Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            Images = await _SystemSettingsDataService.GetSystemImagesAsync(cancellationTokenSource.Token);
        }

        private string GetImageUrl(string imageId) => $"{ImageUrl}{imageId}";
    }
}
