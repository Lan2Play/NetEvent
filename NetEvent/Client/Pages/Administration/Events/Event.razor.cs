using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Administration.Events
{
    public partial class Event
    {
        #region Injects

        [Inject]
        private IEventService EventService { get; set; } = default!;

        [Inject]
        private IVenueService VenueService { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        #endregion

        [Parameter]
        public string? Id { get; set; }

        private const int _HourOffset = 12;
        private const int _DayOffset = 2;

        private bool _Loading = true;
        private EventDto _Event = default!;
        private List<VenueDto> _Venues = new();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _Venues = await VenueService.GetVenuesAsync(cts.Token);

            if (long.TryParse(Id, CultureInfo.InvariantCulture, out var id))
            {
                _Event = await EventService.GetEventAsync(id, cts.Token) ?? new EventDto();
            }
            else
            {
                _Event = new EventDto
                {
                    StartDate = DateTime.Today.AddDays(_DayOffset - 1).AddHours(_HourOffset),
                    EndDate = DateTime.Today.AddDays(_DayOffset).AddHours(_HourOffset),
                };
            }

            _Loading = false;
        }
    }
}
