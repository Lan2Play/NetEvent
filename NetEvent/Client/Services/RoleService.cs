﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public async Task<bool> UpdateRoleAsync(RoleDto updatedRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PutAsJsonAsync($"api/roles/{updatedRole.Id}", updatedRole, cancellationToken);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return false;
        }

        public async Task<bool> AddRoleAsync(RoleDto newRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.PostAsJsonAsync($"api/roles", newRole, cancellationToken);

                response.EnsureSuccessStatusCode();

                newRole.Id = await response.Content.ReadAsStringAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unable to update role in backend.");
            }

            return false;
        }

        public async Task<bool> DeleteRoleAsync(RoleDto deletedRole, CancellationToken cancellationToken)
        {
            try
            {
                var client = _HttpClientFactory.CreateClient(Constants.BackendApiHttpClientName);

                var response = await client.DeleteAsync($"api/roles/{deletedRole.Id}", cancellationToken);

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
