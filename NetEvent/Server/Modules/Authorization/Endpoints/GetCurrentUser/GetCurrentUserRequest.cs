using MediatR;
using System.Security.Claims;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetCurrentUserRequest : IRequest<GetCurrentUserResponse>
    {
        public GetCurrentUserRequest(System.Security.Claims.ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal User { get; }
    }
}
