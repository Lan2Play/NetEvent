using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public static class DeleteRole
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly NetEventRoleManager _RoleManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(NetEventRoleManager roleManager, ILogger<Handler> logger)
            {
                _RoleManager = roleManager;
                _Logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var existingRole = await _RoleManager.FindByIdAsync(request.Id).ConfigureAwait(false);

                if (existingRole == null)
                {
                    return new Response(ReturnType.NotFound, $"Role {request.Id} not found in database.");
                }

                var roleClaims = await _RoleManager.GetClaimsAsync(existingRole);
                foreach (var claimToReomve in roleClaims)
                {
                    await _RoleManager.RemoveClaimAsync(existingRole, claimToReomve);
                }

                await _RoleManager.DeleteAsync(existingRole);

                _Logger.LogDebug("Role deleted {name}", existingRole.Name);

                return new Response();
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string id)
            {
                Guard.IsNotNullOrEmpty(id, nameof(id));

                Id = id;
            }

            public string Id { get; }
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
