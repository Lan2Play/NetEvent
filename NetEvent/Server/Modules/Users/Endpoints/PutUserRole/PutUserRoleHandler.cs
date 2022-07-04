using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Users.Endpoints.PutUserRole
{
    public class PutUserRoleHandler : IRequestHandler<PutUserRoleRequest, PutUserRoleResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;
        private readonly NetEventUserManager _UserManager;
        private readonly ILogger<PutUserRoleHandler> _Logger;

        public PutUserRoleHandler(ApplicationDbContext userDbContext, NetEventUserManager userManager, ILogger<PutUserRoleHandler> logger)
        {
            _UserDbContext = userDbContext;
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<PutUserRoleResponse> Handle(PutUserRoleRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _UserDbContext.FindAsync<ApplicationUser>(new[] { request.UserId }, cancellationToken).ConfigureAwait(false);

            if (existingUser == null)
            {
                return new PutUserRoleResponse(ReturnType.NotFound, $"User {request.UserId} not found in database.");
            }

            var existingRole = await _UserDbContext.FindAsync<ApplicationRole>(new[] { request.RoleId }, cancellationToken).ConfigureAwait(false);

            if (existingRole == null)
            {
                return new PutUserRoleResponse(ReturnType.NotFound, $"Role {request.RoleId} not found in database.");
            }

            var currentRoles = await _UserManager.GetRolesAsync(existingUser);
            await _UserManager.RemoveFromRolesAsync(existingUser, currentRoles);
            await _UserManager.AddToRoleAsync(existingUser, existingRole.Name);

            _Logger.LogInformation("User {UserName} updated", existingUser.UserName);

            return new PutUserRoleResponse();
        }
    }
}
