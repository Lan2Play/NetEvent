using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken);

        Task<ServiceResult> UpdateRoleAsync(RoleDto updatedRole, CancellationToken cancellationToken);

        Task<ServiceResult> AddRoleAsync(RoleDto newRole, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteRoleAsync(RoleDto deletedRole, CancellationToken cancellationToken);
    }
}
