using System.Security.Claims;
using MediatR;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetCurrentUserRequest : IRequest<GetCurrentUserResponse>
    {
        public GetCurrentUserRequest(ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal User { get; }
    }
}
