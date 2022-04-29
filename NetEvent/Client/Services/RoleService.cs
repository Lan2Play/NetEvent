using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public class RoleService : IRoleService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<ThemeService> _Logger;

        public RoleService(IHttpClientFactory httpClientFactory, ILogger<ThemeService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<List<IdentityRole>> GetRolesAsync(CancellationToken cancellationToken)
        {
            var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

            var roles = await client.GetFromJsonAsync<List<IdentityRole>>("/api/roles", cancellationToken).ConfigureAwait(false);

            return roles;
        }

        public async Task<bool> UpdateRoleAsync(IdentityRole updatedRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var content = JsonContent.Create(updatedRole);

                var response = await client.PutAsync($"api/role/{updatedRole.Id}", content, cancellationToken);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }
            return false;
        }
    }
}
