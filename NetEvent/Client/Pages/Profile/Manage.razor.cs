using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StrawberryShake;
using System.Reactive.Linq;

namespace NetEvent.Client.Pages.Profile
{
    public partial class Manage
    {
        [Inject]
        public NetEventClient? NetEventClient { get; set; }
        public IUser User { get; private set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var id = authState.User.Claims.FirstOrDefault(x => x.Type != null && x.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value;
            if(!string.IsNullOrEmpty(id))
            {
                NetEventClient.GetUserById.Watch(id, ExecutionStrategy.CacheFirst)
                                                          .Where(t => !t.Errors.Any())
                                                          .Select(t => t.Data!.User)
                                                          .Subscribe(result =>
                                                          {
                                                              User = result;
                                                              StateHasChanged();
                                                          });

            }
           
            await base.OnInitializedAsync();
        }
    }
}