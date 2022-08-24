using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Administration.Events
{
    public partial class Events
    {
        #region Injects

        [Inject]
        private IEventService _EventService { get; set; } = default!;

        [Inject]
        private ISnackbar _Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> _Localizer { get; set; } = default!;

        #endregion

        private List<EventDto> _Events = new List<EventDto>();
        private bool _Loading = true;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _Events = await _EventService.GetEventsAsync(cts.Token);

            _Loading = false;
        }

        private async Task DeleteEvent(EventDto eventToDelete)
        {
            if (!eventToDelete.Id.HasValue)
            {
                return;
            }

            var cts = new CancellationTokenSource();
            var result = await _EventService.DeleteEventAsync(eventToDelete.Id.Value, cts.Token);

            if (!string.IsNullOrEmpty(result.MessageKey))
            {
                _Snackbar.Add(_Localizer.GetString(result.MessageKey), result.Successful ? Severity.Success : Severity.Error);
            }

            if (result.Successful)
            {
                _Events.Remove(eventToDelete);
            }
        }
    }
}
