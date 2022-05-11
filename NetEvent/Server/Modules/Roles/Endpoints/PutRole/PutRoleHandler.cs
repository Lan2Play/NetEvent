using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Roles.Endpoints.PutRole
{
    public class PutRoleHandler : IRequestHandler<PutRoleRequest, PutRoleResponse>
    {
        private readonly NetEventRoleManager _RoleManager;
        private readonly ILogger<PutRoleHandler> _Logger;

        public PutRoleHandler(NetEventRoleManager roleManager, ILogger<PutRoleHandler> logger)
        {
            _RoleManager = roleManager;
            _Logger = logger;
        }

        public async Task<PutRoleResponse> Handle(PutRoleRequest request, CancellationToken cancellationToken)
        {
            var existingRole = await _RoleManager.FindByIdAsync(request.Id).ConfigureAwait(false);

            if (existingRole == null)
            {
                return new PutRoleResponse(ReturnType.NotFound, $"Role {request.Id} not found in database.");
            }

            // Update existing user
            existingRole.Name = request.Role.Name;
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

            return new PutRoleResponse();
        }
    }
}
