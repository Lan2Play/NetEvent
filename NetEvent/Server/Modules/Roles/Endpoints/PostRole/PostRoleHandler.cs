using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Roles.Endpoints.PostRole
{
    public class PostRoleHandler : IRequestHandler<PostRoleRequest, PostRoleResponse>
    {
        private readonly NetEventRoleManager _RoleManager;
        private readonly ILogger<PostRoleHandler> _Logger;

        public PostRoleHandler(NetEventRoleManager roleManager, ILogger<PostRoleHandler> logger)
        {
            _RoleManager = roleManager;
            _Logger = logger;
        }

        public async Task<PostRoleResponse> Handle(PostRoleRequest request, CancellationToken cancellationToken)
        {
            var existingRole = await _RoleManager.FindByNameAsync(request.Role.Name).ConfigureAwait(false);

            if (existingRole != null)
            {
                return new PostRoleResponse(ReturnType.NotFound, $"Role {request.Role.Name} already exists.");
            }

            var identityRole = DtoMapper.Mapper.RoleDtoToIdentityRole(request.Role);

            await _RoleManager.CreateAsync(identityRole);
            if (request.Role.Claims != null)
            {
                foreach (var claim in request.Role.Claims)
                {
                    await _RoleManager.AddClaimAsync(identityRole, new Claim(claim, string.Empty));
                }
            }

            _Logger.LogDebug("Role created {name}", identityRole.Name);

            return new PostRoleResponse(identityRole.Id);
        }
    }
}
