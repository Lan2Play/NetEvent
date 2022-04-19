using MediatR;
using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class PutUserRequest : IRequest<PutUserResponse>
    {
        public PutUserRequest(string id, ApplicationUser user)
        {
            Id = id;
            User = user;
        }

        public string Id { get; }
        public ApplicationUser User { get; }
    }
}
