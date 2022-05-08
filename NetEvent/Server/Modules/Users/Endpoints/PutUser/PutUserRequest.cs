using MediatR;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

namespace NetEvent.Server.Modules.Users.Endpoints.PutUser
{
    public class PutUserRequest : IRequest<PutUserResponse>
    {
        public PutUserRequest(string id, UserDto user)
        {
            Guard.IsNotNullOrEmpty(id, nameof(id));
            Guard.IsNotNull(user, nameof(user));

            Id = id;
            User = user;
        }

        public string Id { get; }

        public UserDto User { get; }
    }
}
