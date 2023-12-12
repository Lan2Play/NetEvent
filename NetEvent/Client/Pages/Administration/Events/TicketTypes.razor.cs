using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Administration.Events
{
    public partial class TicketTypes
    {
        #region Injects

        [Inject]
        private IStringLocalizer<App> Localizer { get; set; } = default!;

        #endregion

        [Parameter]
        public EventDto Event { get; set; } = default!;
    }
}
