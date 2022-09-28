using System;
using System.Collections.Generic;
using System.Linq;
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
        private IEventService _EventService { get; set; } = default!;

        [Inject]
        private IVenueService _VenueService { get; set; } = default!;

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        [Parameter]
        public string? Id { get; set; }

        private readonly EventModelFluentValidator _EventValidator = new();
        private bool _Loading = true;
        private MudForm? _Form = default!;
        private EventDto _Event = default!;
        private List<VenueDto> _Venues = new();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();
            _Venues = await _VenueService.GetVenuesAsync(cts.Token);

            if (long.TryParse(Id, out var id))
            {
                _Event = await _EventService.GetEventAsync(id, cts.Token) ?? new EventDto();
            }
            else
            {
                _Event = new EventDto
                {
                    StartDate = DateTime.Today.AddDays(1).AddHours(12),
                    EndDate = DateTime.Today.AddDays(2).AddHours(12),
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
                    result = await _EventService.UpdateEventAsync(_Event, cts.Token);
                }
                else
                {
                    result = await _EventService.CreateEventAsync(_Event, cts.Token);

                    if (result.Successful && _Event?.Id != null)
                    {
                        NavigationManager.NavigateTo(UrlHelper.GetEventLink(_Event.Id, true));
                    }
                }

                if (!string.IsNullOrEmpty(result.MessageKey))
                {
                    _Snackbar.Add(_Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
                }
            }
        }
    }
}
