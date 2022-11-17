using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public static class PutUserRole
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _UserDbContext;
            private readonly NetEventUserManager _UserManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(ApplicationDbContext userDbContext, NetEventUserManager userManager, ILogger<Handler> logger)
            {
                _UserDbContext = userDbContext;
                _UserManager = userManager;
                _Logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var existingUser = await _UserDbContext.FindAsync<ApplicationUser>(new[] { request.UserId }, cancellationToken).ConfigureAwait(false);

                if (existingUser == null)
                {
                    return new Response(ReturnType.NotFound, $"User {request.UserId} not found in database.");
                }

                var existingRole = await _UserDbContext.FindAsync<ApplicationRole>(new[] { request.RoleId }, cancellationToken).ConfigureAwait(false);

                if (string.IsNullOrEmpty(existingRole?.Name))
                {
                    return new Response(ReturnType.NotFound, $"Role {request.RoleId} not found in database.");
                }

                var currentRoles = await _UserManager.GetRolesAsync(existingUser);
                await _UserManager.RemoveFromRolesAsync(existingUser, currentRoles);
                await _UserManager.AddToRoleAsync(existingUser, existingRole.Name);

                _Logger.LogInformation("User {UserName} updated", existingUser.UserName);

                return new Response();
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string userId, string roleId)
            {
                Guard.IsNotNullOrEmpty(userId, nameof(userId));
                Guard.IsNotNullOrEmpty(roleId, nameof(roleId));

                UserId = userId;
                RoleId = roleId;
            }

            public string UserId { get; }

            public string RoleId { get; }
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
