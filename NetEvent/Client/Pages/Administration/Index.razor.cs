using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto.Administration;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Pages.Administration
{
    public partial class Index
    {
        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IEventService EventService { get; set; } = default!;

        public List<AdminUserDto>? Users { get; private set; }

        public List<EventDto>? Events { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var loadUsers = UserService.GetUsersAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            var loadEvents = EventService.GetEventsAsync(cancellationTokenSource.Token).ConfigureAwait(false);

            Users = await loadUsers;
            Events = await loadEvents;
        }
    }
}
