using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
        Task<bool> UpdateUserAsync(UserDto updatedUser, CancellationToken cancellationToken);
    }
}