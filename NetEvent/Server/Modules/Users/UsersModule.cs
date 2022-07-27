using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Users.Endpoints;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users
{
    public class UsersModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/users", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetUsers.Request())));
            endpoints.MapGet("/api/users/{userId}", async ([FromRoute] string userId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUser.Request(userId))));
            endpoints.MapPut("/api/users/{userId}", async ([FromRoute] string userId, [FromBody] UserDto user, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUser.Request(userId, user))));
            endpoints.MapPut("/api/users/{userId}/role", async ([FromRoute] string userId, [FromBody] string roleId, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUserRole.Request(userId, roleId))));
            endpoints.MapGet("/api/users/{userId}/email/confirm", async ([FromRoute] string userId, [FromQuery] string code, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUserEmailConfirm.Request(userId, code))));
            return endpoints;
        }
    }
}
