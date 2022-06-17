using MediatR;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles.Endpoints.PostRole
{
    public class PostRoleRequest : IRequest<PostRoleResponse>
    {
        public PostRoleRequest(RoleDto role)
        {
            Guard.IsNotNull(role, nameof(role));

            Role = role;
        }

        public RoleDto Role { get; }
    }
}
