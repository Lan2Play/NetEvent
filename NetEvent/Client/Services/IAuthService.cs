using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        Task<CurrentUserDto> CurrentUserInfo();
    }
}
