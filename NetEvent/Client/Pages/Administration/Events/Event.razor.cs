using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Event;
using NetEvent.Shared.Validators;
using static MudBlazor.CategoryTypes;

namespace NetEvent.Client.Pages.Administration.Events
{
    public partial class Event
    {
        #region Injects

        [Inject]
        private IEventService _EventService { get; set; } = default!;

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        #endregion

        [Parameter]
        public string? Id { get; set; }

        private readonly EventModelFluentValidator _EventValidator = new();
        private bool _Loading = true;
        private MudForm _Form = default!;
        private EventDto _Event = default!;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            if (long.TryParse(Id, out var id))
            {
                _Event = await _EventService.GetEventAsync(id, cts.Token) ?? new EventDto();
            }
            else
            {
                _Event = new EventDto();
            }

            _Loading = false;
        }

        private async Task SaveEvent()
        {
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
                }

                if (!string.IsNullOrEmpty(result.MessageKey))
                {
                    _Snackbar.Add(_Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
                }
            }
        }
    }
}
