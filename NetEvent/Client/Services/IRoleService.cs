using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NetEvent.Client.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetRolesAsync(CancellationToken cancellationToken);
        Task<bool> UpdateRoleAsync(IdentityRole updatedRole, CancellationToken cancellationToken);
    }
}
