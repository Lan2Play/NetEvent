using System.Security.Claims;
using MediatR;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetUserRequest : IRequest<GetUserResponse>
    {
        public GetUserRequest(ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal User { get; }
    }
}
