using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Roles.Endpoints;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles
{
    [ExcludeFromCodeCoverage]
    public class RolesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/roles", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetRoles.Request())));
            endpoints.MapPut("/api/roles/{roleId}", async ([FromRoute] string roleId, [FromBody] RoleDto role, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutRole.Request(roleId, role))));
            endpoints.MapDelete("/api/roles/{roleId}", async ([FromRoute] string roleId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteRole.Request(roleId))));
            endpoints.MapPost("/api/roles", async ([FromBody] RoleDto role, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRole.Request(role))));
            return endpoints;
        }
    }
}
