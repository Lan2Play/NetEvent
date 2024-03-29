﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Administration.Venues
{
    public partial class Venues
    {
        #region Injects

        [Inject]
        private IVenueService VenueService { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        #endregion

        private List<VenueDto> _Venues = new();
        private bool _Loading = true;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _Venues = await VenueService.GetVenuesAsync(cts.Token);

            _Loading = false;
        }

        private async Task DeleteVenue(VenueDto venueToDelete)
        {
            if (!venueToDelete.Id.HasValue)
            {
                return;
            }

            var cts = new CancellationTokenSource();
            var result = await VenueService.DeleteVenueAsync(venueToDelete.Id.Value, cts.Token);

            if (!string.IsNullOrEmpty(result.MessageKey))
            {
                Snackbar.Add(Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
            }

            if (result.Successful)
            {
                _Venues.Remove(venueToDelete);
            }
        }
    }
}
