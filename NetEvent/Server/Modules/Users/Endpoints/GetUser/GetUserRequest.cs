using MediatR;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
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
