using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Roles.Endpoints.GetRoles;
using NetEvent.Server.Modules.Roles.Endpoints.PostRole;
using NetEvent.Server.Modules.Roles.Endpoints.PutRole;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles
{
    [ExcludeFromCodeCoverage]
    public class RolesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/roles", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetRolesRequest())));
            endpoints.MapPut("/api/roles/{roleId}", async ([FromRoute] string roleId, [FromBody] RoleDto role, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutRoleRequest(roleId, role))));
            endpoints.MapPost("/api/roles", async ([FromBody] RoleDto role, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRoleRequest(role))));
            return endpoints;
        }
    }
}
