using System.Collections.Generic;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles.Endpoints.GetRoles
{
    public class GetRolesResponse : ResponseBase<IReadOnlyCollection<RoleDto>>
    {
        public GetRolesResponse(IReadOnlyCollection<RoleDto> value) : base(value)
        {
        }

        public GetRolesResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
