using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

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

        public async Task<List<AdminUserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var users = await client.GetFromJsonAsync<List<AdminUserDto>>("api/users", cancellationToken).ConfigureAwait(false);

                if (users == null)
                {
                    _Logger.LogError("Unable to get users data from backend");
                    return new List<AdminUserDto>();
                }

                return users;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get users data from backend");
                return new List<AdminUserDto>();
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
