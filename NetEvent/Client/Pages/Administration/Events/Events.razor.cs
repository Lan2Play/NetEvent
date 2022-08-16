using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
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

        #endregion

        private List<EventDto> _Events = new List<EventDto>();
        private bool _Loading = true;

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _Events = await _EventService.GetEventsAsync(cts.Token);

            _Loading = false;
        }

        private string GetEventLink(object id)
        {
            return $"/administration/event/{id}";
        }
    }
}
