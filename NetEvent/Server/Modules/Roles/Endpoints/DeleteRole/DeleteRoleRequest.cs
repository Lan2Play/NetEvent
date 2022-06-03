using MediatR;
using Microsoft.Toolkit.Diagnostics;

namespace NetEvent.Server.Modules.Roles.Endpoints.DeleteRole
{
    public class DeleteRoleRequest : IRequest<DeleteRoleResponse>
    {
        public DeleteRoleRequest(string id)
        {
            Guard.IsNotNullOrEmpty(id, nameof(id));

            Id = id;
        }

        public string Id { get; }
    }
}
