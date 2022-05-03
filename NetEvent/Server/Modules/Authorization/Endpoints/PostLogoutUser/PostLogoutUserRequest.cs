using MediatR;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser
{
    public class PostLogoutUserRequest : IRequest<PostLogoutUserResponse>
    {
    }
}
