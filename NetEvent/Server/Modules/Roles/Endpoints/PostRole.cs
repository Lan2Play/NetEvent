using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public static class PostRole
    {
        public sealed class Handler : IRequestHandler<Request, Response>
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
                var existingRole = await _RoleManager.FindByNameAsync(request.Role.Name).ConfigureAwait(false);

                if (existingRole != null)
                {
                    return new Response(ReturnType.Error, $"Role {request.Role.Name} already exists.");
                }

                var applicationRole = request.Role.ToApplicationRole();
                applicationRole.Id = applicationRole.Name!.ToLowerInvariant();
                applicationRole.NormalizedName = applicationRole.Name.ToUpperInvariant();

                var createResult = await _RoleManager.CreateAsync(applicationRole);

                if (!createResult.Succeeded)
                {
                    return new Response(ReturnType.NotFound, $"Error creating Role {request.Role.Name}. {string.Join(", ", createResult.Errors.Select(x => x.Description))}");
                }

                if (request.Role.Claims != null)
                {
                    foreach (var claim in request.Role.Claims)
                    {
                        await _RoleManager.AddClaimAsync(applicationRole, new Claim(claim, string.Empty));
                    }
                }

                _Logger.LogDebug("Role created {name}", applicationRole.Name);

                return new Response(applicationRole.Id);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(RoleDto role)
            {
                Guard.IsNotNull(role, nameof(role));

                Role = role;
            }

            public RoleDto Role { get; }
        }

        public sealed class Response : ResponseBase<string>
        {
            public Response(string id) : base(id)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
