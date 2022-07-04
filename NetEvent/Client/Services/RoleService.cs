using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
    public class RoleService : IRoleService
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<ThemeService> _Logger;

        public RoleService(IHttpClientFactory httpClientFactory, ILogger<ThemeService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var roles = await client.GetFromJsonAsync<List<RoleDto>>("/api/roles", cancellationToken).ConfigureAwait(false);

                if (roles == null)
                {
                    _Logger.LogError("Unable to get roles data from backend");
                    return new List<RoleDto>();
                }

                return roles;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to get roles data from backend");
                return new List<RoleDto>();
            }
        }

        public async Task<ServiceResult> UpdateRoleAsync(RoleDto updatedRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/roles/{updatedRole.Id}", updatedRole, cancellationToken);

                response.EnsureSuccessStatusCode();

                return ServiceResult.Success("RoleService.UpdateRoleAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return ServiceResult.Error("RoleService.UpdateRoleAsync.Error");
        }

        public async Task<ServiceResult> AddRoleAsync(RoleDto newRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/roles", newRole, cancellationToken);

                response.EnsureSuccessStatusCode();

                newRole.Id = await response.Content.ReadAsStringAsync(cancellationToken);

                return ServiceResult.Success("RoleService.AddRoleAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return ServiceResult.Error("RoleService.AddRoleAsync.Error");
        }

        public async Task<ServiceResult> DeleteRoleAsync(RoleDto deletedRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.DeleteAsync($"api/roles/{deletedRole.Id}", cancellationToken);

                response.EnsureSuccessStatusCode();
                return ServiceResult.Success("RoleService.DeleteRoleAsync.Success");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return ServiceResult.Error("RoleService.DeleteRoleAsync.Error");
        }
    }
}
