using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Server.Data;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public static class PutRole
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

                // Update existing role
                existingRole.Name = request.Role.Name;
                existingRole.IsDefault = request.Role.IsDefault;
                existingRole.NormalizedName = request.Role.Name.ToUpperInvariant();

                await _RoleManager.UpdateAsync(existingRole);

                var roleClaims = await _RoleManager.GetClaimsAsync(existingRole);
                var claimsToRemove = request.Role.Claims != null ? roleClaims.Where(x => !request.Role.Claims.Contains(x.Type, StringComparer.OrdinalIgnoreCase)) : Enumerable.Empty<Claim>();
                foreach (var claimToReomve in claimsToRemove)
                {
                    await _RoleManager.RemoveClaimAsync(existingRole, claimToReomve);
                }

                var claimsToAdd = request.Role.Claims != null ? request.Role.Claims.Where(x => !roleClaims.Select(c => c.Type).Contains(x, StringComparer.OrdinalIgnoreCase)) : Enumerable.Empty<string>();

                foreach (var newClaim in claimsToAdd)
                {
                    await _RoleManager.AddClaimAsync(existingRole, new Claim(newClaim, string.Empty));
                }

                _Logger.LogDebug("Role updated {name}", existingRole.Name);

                return new Response();
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string id, RoleDto role)
            {
                Guard.IsNotNullOrEmpty(id, nameof(id));
                Guard.IsNotNull(role, nameof(role));

                Id = id;
                Role = role;
            }

            public string Id { get; }

            public RoleDto Role { get; }
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
