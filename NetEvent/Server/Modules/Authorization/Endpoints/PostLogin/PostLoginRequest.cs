using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogin
{
    public class PostLoginRequest : IRequest<PostLoginResponse>
    {
        public PostLoginRequest(LoginRequestDto loginRequest)
        {
            LoginRequest = loginRequest;
        }

        public LoginRequestDto LoginRequest { get; }
    }
}
