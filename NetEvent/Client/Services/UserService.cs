using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
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

        public async Task<ServiceResult> UpdateUserAsync(UserDto updatedUser, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/users/{updatedUser.Id}", updatedUser, cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("UserService.UpdateUserAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update user in backend.");
            }

            return ServiceResult.Error("UserService.UpdateUserAsync.Error");
        }

        public async Task<ServiceResult> UpdateUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/users/{userId}/role", roleId, cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("UserService.UpdateUserAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update user in backend.");
            }

            return ServiceResult.Error("UserService.UpdateUserAsync.Error");
        }
    }
}
