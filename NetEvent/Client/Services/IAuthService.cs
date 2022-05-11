using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);

        Task<ServiceResult> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);

        Task LogoutAsync(CancellationToken cancellationToken);

        Task<CurrentUserDto> GetCurrentUserInfoAsync(CancellationToken cancellationToken);

        Task CompleteRegistrationAsync(CompleteRegistrationRequestDto completeRegistrationRequest, CancellationToken cancellationToken);
    }
}
