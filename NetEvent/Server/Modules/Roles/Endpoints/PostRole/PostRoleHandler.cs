using System.Linq;
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
                return new PostRoleResponse(ReturnType.Error, $"Role {request.Role.Name} already exists.");
            }

            var applicationRole = DtoMapper.Mapper.RoleDtoToApplicationRole(request.Role);
            applicationRole.Id = applicationRole.Name.ToLowerInvariant();
            applicationRole.NormalizedName = applicationRole.Name.ToUpperInvariant();

            var createResult = await _RoleManager.CreateAsync(applicationRole);

            if (!createResult.Succeeded)
            {
                return new PostRoleResponse(ReturnType.NotFound, $"Error creating Role {request.Role.Name}. {string.Join(", ", createResult.Errors.Select(x => x.Description))}");
            }

            if (request.Role.Claims != null)
            {
                foreach (var claim in request.Role.Claims)
                {
                    await _RoleManager.AddClaimAsync(applicationRole, new Claim(claim, string.Empty));
                }
            }

            _Logger.LogDebug("Role created {name}", applicationRole.Name);

            return new PostRoleResponse(applicationRole.Id);
        }
    }
}
