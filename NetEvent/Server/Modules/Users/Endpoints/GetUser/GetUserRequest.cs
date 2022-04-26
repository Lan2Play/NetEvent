using MediatR;
using Microsoft.Toolkit.Diagnostics;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserRequest : IRequest<GetUserResponse>
    {
        public GetUserRequest(string id)
        {
            Guard.IsNotNullOrEmpty(id, nameof(id));
            Id = id;
        }

        public string Id { get; }
    }
}
