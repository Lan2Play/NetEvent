using System.Net.Http.Json;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<ThemeService> _Logger;

        public UserService(IHttpClientFactory httpClientFactory, ILogger<ThemeService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {

            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var users = await client.GetFromJsonAsync<List<UserDto>>("api/users", cancellationToken).ConfigureAwait(false);

                if (users == null)
                {
                    _Logger.LogError("Unable to get users data from backend");
                    return new List<UserDto>();
                }

                return users;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get users data from backend");
                return new List<UserDto>();
            }
        }

        public async Task<bool> UpdateUserAsync(UserDto updatedUser, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var content = JsonContent.Create(updatedUser);

                var response = await client.PutAsync($"api/user/{updatedUser.Id}", content, cancellationToken);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update user in backend.");
            }
            return false;
        }
    }
}
