using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Extensions;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Event;
using NetEvent.Shared.Validators;

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
        private ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        [Parameter]
        public string? Id { get; set; }

        private const int _HourOffset = 12;
        private const int _DayOffset = 2;

        private readonly EventModelFluentValidator _EventValidator = new();
        private bool _Loading = true;
        private MudForm? _Form = default!;
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

        private async Task SaveEvent()
        {
            if (_Form == null)
            {
                return;
            }

            await _Form.Validate();

            if (_Form.IsValid)
            {
                var cts = new CancellationTokenSource();
                ServiceResult result;
                if (_Event.Id >= 0)
                {
                    result = await EventService.UpdateEventAsync(_Event, cts.Token);
                }
                else
                {
                    result = await EventService.CreateEventAsync(_Event, cts.Token);

                    if (result.Successful && _Event?.Id != null)
                    {
                        NavigationManager.NavigateTo(UrlHelper.GetEventLink(_Event.Id, true));
                    }
                }

                if (!string.IsNullOrEmpty(result.MessageKey))
                {
                    Snackbar.Add(Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
                }
            }
        }
    }
}
