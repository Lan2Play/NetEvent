using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Shared
{
    public partial class NavigationBar
    {
        #region Injects

        [Inject]
        private IEventService _EventService { get; set; } = default!;

        #endregion

        private List<EventDto> _Events = new List<EventDto>();

        protected override async Task OnInitializedAsync()
        {
            var cts = new CancellationTokenSource();

            _Events = await _EventService.GetEventsAsync(cts.Token);
        }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.Logout();
            Navigation.NavigateTo("/");
        }

        private string GetEventUrl(EventDto eventDto)
        {
            return $"/event/{eventDto.Slug}";
        }
    }
}
