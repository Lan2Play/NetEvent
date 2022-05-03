using NetEvent.Shared.Dto;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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

        public async Task<CurrentUserDto> CurrentUserInfo()
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(_HttpClientName);

                var result = await client.GetFromJsonAsync<CurrentUserDto>("api/auth/user/current");
                return result;
            }
            catch (Exception ex)
            {
                return new CurrentUserDto() { IsAuthenticated = false };
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
