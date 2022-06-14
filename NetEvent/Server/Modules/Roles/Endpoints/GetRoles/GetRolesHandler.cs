using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Roles.Endpoints.GetRoles
{
    public class GetRolesHandler : IRequestHandler<GetRolesRequest, GetRolesResponse>
    {
        private readonly NetEventRoleManager _RoleManager;

        public GetRolesHandler(NetEventRoleManager roleManager)
        {
            _RoleManager = roleManager;
        }

        public Task<GetRolesResponse> Handle(GetRolesRequest request, CancellationToken cancellationToken)
        {
            var allRoles = _RoleManager.Roles.ToList();
            var roleDtos = allRoles.Select(async role =>
            {
                var roleDto = DtoMapper.Mapper.ApplicationRoleToRoleDto(role);
                var roleClaims = await _RoleManager.GetClaimsAsync(role);
                roleDto.Claims = roleClaims.Select(roleClaim => roleClaim.Type).ToList();
                return roleDto;
            }).Select(t => t.Result).ToList();

            return Task.FromResult(new GetRolesResponse(roleDtos));
        }
    }
}
