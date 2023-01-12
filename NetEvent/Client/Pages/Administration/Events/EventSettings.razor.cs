using System.Collections.Generic;
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
    public partial class EventSettings
    {
        #region Injects

        [Inject]
        private IEventService EventService { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        [Parameter]
        public EventDto Event { get; set; } = default!;

        [Parameter]
        public List<VenueDto> Venues { get; set; } = default!;

        private readonly EventModelFluentValidator _EventValidator = new();

        private MudForm? _Form = default!;

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
                if (Event.Id >= 0)
                {
                    result = await EventService.UpdateEventAsync(Event, cts.Token);
                }
                else
                {
                    result = await EventService.CreateEventAsync(Event, cts.Token);

                    if (result.Successful && Event?.Id != null)
                    {
                        NavigationManager.NavigateTo(UrlHelper.GetEventLink(Event.Id, true));
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
