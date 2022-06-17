using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Roles.Endpoints.DeleteRole
{
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleRequest, DeleteRoleResponse>
    {
        private readonly NetEventRoleManager _RoleManager;
        private readonly ILogger<DeleteRoleHandler> _Logger;

        public DeleteRoleHandler(NetEventRoleManager roleManager, ILogger<DeleteRoleHandler> logger)
        {
            _RoleManager = roleManager;
            _Logger = logger;
        }

        public async Task<DeleteRoleResponse> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
        {
            var existingRole = await _RoleManager.FindByIdAsync(request.Id).ConfigureAwait(false);

            if (existingRole == null)
            {
                return new DeleteRoleResponse(ReturnType.NotFound, $"Role {request.Id} not found in database.");
            }

            var roleClaims = await _RoleManager.GetClaimsAsync(existingRole);
            foreach (var claimToReomve in roleClaims)
            {
                await _RoleManager.RemoveClaimAsync(existingRole, claimToReomve);
            }

            await _RoleManager.DeleteAsync(existingRole);

            _Logger.LogDebug("Role deleted {name}", existingRole.Name);

            return new DeleteRoleResponse();
        }
    }
}
