using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken);

        Task<bool> UpdateRoleAsync(RoleDto updatedRole, CancellationToken cancellationToken);

        Task<bool> AddRoleAsync(RoleDto newRole, CancellationToken cancellationToken);
    }
}
