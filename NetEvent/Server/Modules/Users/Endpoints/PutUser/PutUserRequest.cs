using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.PutUser
{
    public class PutUserRequest : IRequest<PutUserResponse>
    {
        public PutUserRequest(string id, CurrentUser user)
        {
            Id = id;
            User = user;
        }

        public string Id { get; }
        public CurrentUser User { get; }
    }
}
