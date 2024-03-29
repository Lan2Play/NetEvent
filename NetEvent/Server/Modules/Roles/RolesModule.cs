﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Roles.Endpoints;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Roles
{
    public class RolesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapModuleEndpoints(IEndpointRouteBuilder endpoints)
        {
            // BaseRoute: /api/roles
            endpoints.MapGet("/", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetRoles.Request())));
            endpoints.MapPut("/{roleId}", async ([FromRoute] string roleId, [FromBody] RoleDto role, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutRole.Request(roleId, role))));
            endpoints.MapDelete("/{roleId}", async ([FromRoute] string roleId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteRole.Request(roleId))));
            endpoints.MapPost("/", async ([FromBody] RoleDto role, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRole.Request(role))));
            return endpoints;
        }
    }
}
