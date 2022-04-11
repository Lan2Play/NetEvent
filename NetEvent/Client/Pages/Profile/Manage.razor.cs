using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NetEvent.Client.Models;
using System.Reactive.Linq;

namespace NetEvent.Client.Pages.Profile
{
    public partial class Manage
    {

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

                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public void UpdateUser()
        {

        }
    }
}