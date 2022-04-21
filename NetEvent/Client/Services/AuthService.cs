using NetEvent.Shared.Dto;
using System.Net.Http.Json;

namespace NetEvent.Client.Services
{
    public class AuthService : IAuthService
    {
        private const string _HttpClientName = "NetEvent.ServerAPI";

        private readonly IHttpClientFactory _HttpClientFactory;
        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _HttpClientFactory = httpClientFactory;
        }
        public async Task<CurrentUser> CurrentUserInfo()
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(_HttpClientName);

                var result = await client.GetFromJsonAsync<CurrentUser>("api/auth/user/current");
                return result;
            }
            catch (Exception ex)
            {

                return new CurrentUser() { IsAuthenticated = false};
            }
        }

        public async Task Login(LoginRequest loginRequest)
        {
            var client = _HttpClientFactory.CreateClient(_HttpClientName);

            var result = await client.PostAsJsonAsync("api/auth/login", loginRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
        public async Task Logout()
        {
            var client = _HttpClientFactory.CreateClient(_HttpClientName);
            var result = await client.PostAsync("api/auth/logout", null);
            result.EnsureSuccessStatusCode();
        }
        public async Task Register(RegisterRequest registerRequest)
        {
            var client = _HttpClientFactory.CreateClient(_HttpClientName);
            var result = await client.PostAsJsonAsync("api/auth/register", registerRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
    }
}
