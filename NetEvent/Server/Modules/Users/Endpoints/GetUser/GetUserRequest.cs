using MediatR;
using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetUserRequest : IRequest<GetUserResponse>
    {
        public GetUserRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
