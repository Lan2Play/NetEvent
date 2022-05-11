using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser
{
    public class PostLoginRequest : IRequest<PostLoginResponse>
    {
        public PostLoginRequest()
        {
        }

        public PostLoginRequest(LoginRequestDto loginRequest)
        {
            LoginRequest = loginRequest;
        }

        public LoginRequestDto LoginRequest { get; }
    }
}
