using Microsoft.AspNetCore.Components.Authorization;
using NetEvent.Shared.Dto;
using System.Security.Claims;

namespace NetEvent.Client.Services
{
    public class NetEventAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _Api;

        private CurrentUserDto? _CurrentUser;

        public NetEventAuthenticationStateProvider(IAuthService api)
        {
            _Api = api;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo != null && userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _CurrentUser.UserName) }.Concat(_CurrentUser.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
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

        public async Task Login(LoginRequest loginParameters)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await _Api.LoginAsync(loginParameters, cancellationTokenSource.Token).ConfigureAwait(false);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterRequest registerParameters)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await _Api.RegisterAsync(registerParameters, cancellationTokenSource.Token).ConfigureAwait(false);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
