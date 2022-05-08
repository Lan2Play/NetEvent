using MediatR;
using Microsoft.Toolkit.Diagnostics;

namespace NetEvent.Server.Modules.Users.Endpoints.PutUserRole
{
    public class PutUserRoleRequest : IRequest<PutUserRoleResponse>
    {
        public PutUserRoleRequest(string userId, string roleId)
        {
            Guard.IsNotNullOrEmpty(userId, nameof(userId));
            Guard.IsNotNullOrEmpty(roleId, nameof(roleId));

            UserId = userId;
            RoleId = roleId;
        }

        public string UserId { get; }

        public string RoleId { get; }
    }
}
