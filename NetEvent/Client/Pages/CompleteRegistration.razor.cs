using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using NetEvent.Client.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Pages
{
    public partial class CompleteRegistration
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IAuthService _AuthService { get; set; } = default!;

        [Inject]
        private ILogger<CompleteRegistration> _Logger { get; set; } = default!;

        [Inject]
        private NetEventAuthenticationStateProvider _NetEventAuthenticationStateProvider { get; set; } = default!;

        public CompleteRegistrationRequestDto CompleteRegistrationRequest { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authenticationState = await _NetEventAuthenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);

                var userId = authenticationState.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                CompleteRegistrationRequest.UserId = userId;
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task CompleteRegistrationAsync()
        {
            try
            {
                await _AuthService.CompleteRegistrationAsync(CompleteRegistrationRequest, CancellationToken.None);
                NavigationManager.NavigateTo(string.Empty);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error completing registration.");
            }
        }
    }
}
