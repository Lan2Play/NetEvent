using MediatR;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles.Endpoints.PutRole
{
    public class PutRoleRequest : IRequest<PutRoleResponse>
    {
        public PutRoleRequest(string id, RoleDto role)
        {
            Guard.IsNotNullOrEmpty(id, nameof(id));
            Guard.IsNotNull(role, nameof(role));

            Id = id;
            Role = role;
        }

        public string Id { get; }

        public RoleDto Role { get; }
    }
}
