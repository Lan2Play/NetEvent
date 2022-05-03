using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser
{
    public class PostLoginUserRequest : IRequest<PostLoginUserResponse>
    {
        public PostLoginUserRequest()
        {
        }

        public PostLoginUserRequest(LoginRequest loginRequest)
        {
            LoginRequest = loginRequest;
        }

        public LoginRequest LoginRequest { get; }
    }
}
