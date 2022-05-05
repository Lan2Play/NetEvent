using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

namespace NetEvent.Client.Services
{
    public interface IUserService
    {
        Task<List<AdminUserDto>> GetUsersAsync(CancellationToken cancellationToken);

        Task<ServiceResult> UpdateUserAsync(UserDto updatedUser, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken);
    }
}
