using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NetEvent.Client.Models;
using StrawberryShake;
using System.Reactive.Linq;

namespace NetEvent.Client.Pages.Profile
{
    public partial class Manage
    {
        [Inject]
        public NetEventClient? NetEventClient { get; set; }
        public User? User { get; private set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthenticationStateTask;
                var id = authState.User.Claims.FirstOrDefault(x => x.Type != null && x.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value;
                if (!string.IsNullOrEmpty(id))
                {
                    NetEventClient!.GetUserById.Watch(id, ExecutionStrategy.CacheFirst)
                                                              .Subscribe(result =>
                                                              {
                                                                  if (result.Data?.User != null)
                                                                  {
                                                                      User = result.Data.User.ToUser();
                                                                  }

                                                                  StateHasChanged();
                                                              });

                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public void UpdateUser()
        {

        }
    }
}