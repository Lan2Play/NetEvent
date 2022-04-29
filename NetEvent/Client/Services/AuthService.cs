using NetEvent.Shared.Dto;
using System.Net.Http.Json;

namespace NetEvent.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<AuthService> _Logger;

        public AuthService(IHttpClientFactory httpClientFactory, ILogger<AuthService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<CurrentUserDto> GetCurrentUserInfoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var result = await client.GetFromJsonAsync<CurrentUserDto>("api/auth/user/current", cancellationToken);

                if(result == null)
                {
                    _Logger.LogError("Unable to get current user from backend.");
                    return new CurrentUserDto() { IsAuthenticated = false };
                }

                return result;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get current user from backend.");
                return new CurrentUserDto() { IsAuthenticated = false};
            }
        }

        public async Task LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

            var result = await client.PostAsJsonAsync("api/auth/login", loginRequest, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);
            var result = await client.PostAsync("api/auth/logout", null, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
        public async Task RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);
            var result = await client.PostAsJsonAsync("api/auth/register", registerRequest, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
    }
}
