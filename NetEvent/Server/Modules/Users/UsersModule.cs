using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Users.Endpoints;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Policy;

namespace NetEvent.Server.Modules.Users
{
    public class UsersModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/users", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetUsers.Request()))).RequireAuthorization(Policy.AdminUsersRead);
            endpoints.MapGet("/api/users/{userId}", async ([FromRoute] string userId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUser.Request(userId)))).RequireAuthorization(Policy.AdminUsersRead);
            endpoints.MapPut("/api/users/{userId}", async ([FromRoute] string userId, [FromBody] UserDto user, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUser.Request(userId, user)))).RequireAuthorization(Policy.AdminUsersEdit);
            endpoints.MapPut("/api/users/{userId}/role", async ([FromRoute] string userId, [FromBody] string roleId, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUserRole.Request(userId, roleId)))).RequireAuthorization(Policy.AdminUsersEdit);
            endpoints.MapGet("/api/users/{userId}/email/confirm", async ([FromRoute] string userId, [FromQuery] string code, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUserEmailConfirm.Request(userId, code)))).RequireAuthorization(Policy.AdminUsersEdit);
            return endpoints;
        }
    }
}
