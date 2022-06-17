using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public class NetEventAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _Api;
        private readonly ILogger<ThemeService> _Logger;

        private CurrentUserDto? _CurrentUser;

        public NetEventAuthenticationStateProvider(IAuthService api, ILogger<ThemeService> logger)
        {
            _Api = api;
            _Logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo?.UserName != null && userInfo?.Claims != null && userInfo.IsAuthenticated)
                {
                    var claims = userInfo.Claims.Select(c => new Claim(c.Key, c.Value));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get identity");
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task<CurrentUserDto> GetCurrentUser()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            if (_CurrentUser != null && _CurrentUser.IsAuthenticated)
            {
                return _CurrentUser;
            }

            _CurrentUser = await _Api.GetCurrentUserInfoAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            return _CurrentUser;
        }

        public async Task Logout()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await _Api.LogoutAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            _CurrentUser = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<ServiceResult> Login(LoginRequestDto loginParameters)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var result = await _Api.LoginAsync(loginParameters, cancellationTokenSource.Token).ConfigureAwait(false);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return result;
        }

        public async Task<ServiceResult> Register(RegisterRequestDto registerParameters)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var result = await _Api.RegisterAsync(registerParameters, cancellationTokenSource.Token).ConfigureAwait(false);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return result;
        }
    }
}
