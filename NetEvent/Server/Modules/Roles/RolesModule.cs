using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Modules.Roles.Endpoints;

namespace NetEvent.Server.Modules.Roles
{
    [ExcludeFromCodeCoverage]
    public class RolesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/roles", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetRolesRequest())));
            return endpoints;
        }
    }
}
