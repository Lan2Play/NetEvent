using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Server.Data;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public static class PutUser
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly NetEventUserManager _UserManager;

            public Handler(NetEventUserManager userManager)
            {
                _UserManager = userManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = request.User;

                var existingUser = await _UserManager.FindByIdAsync(request.Id).ConfigureAwait(false);

                if (existingUser == null)
                {
                    return new Response(ReturnType.NotFound, $"User {request.Id} not found in database.");
                }

                // Update existing user
                existingUser.UserName = user.UserName;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.EmailConfirmed = user.EmailConfirmed;

                await _UserManager.UpdateAsync(existingUser);

                return new Response();
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string id, UserDto user)
            {
                Guard.IsNotNullOrEmpty(id, nameof(id));
                Guard.IsNotNull(user, nameof(user));

                Id = id;
                User = user;
            }

            public string Id { get; }

            public UserDto User { get; }
        }

        public class Response : ResponseBase
        {
            public Response()
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
