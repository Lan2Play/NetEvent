using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResult> LoginAsync(LoginRequestDto loginRequest, CancellationToken cancellationToken);

        Task<ServiceResult> RegisterAsync(RegisterRequestDto registerRequest, CancellationToken cancellationToken);

        Task LogoutAsync(CancellationToken cancellationToken);

        Task<CurrentUserDto> GetCurrentUserInfoAsync(CancellationToken cancellationToken);

        Task CompleteRegistrationAsync(RegisterExternalCompleteRequestDto completeRegistrationRequest, CancellationToken cancellationToken);
    }
}
