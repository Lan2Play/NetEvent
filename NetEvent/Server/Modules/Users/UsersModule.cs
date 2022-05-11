using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Users.Endpoints.GetUser;
using NetEvent.Server.Modules.Users.Endpoints.GetUsers;
using NetEvent.Server.Modules.Users.Endpoints.PutUser;
using NetEvent.Server.Modules.Users.Endpoints.PutUserRole;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users
{
    public class UsersModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/users", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetUsersRequest())));
            endpoints.MapGet("/api/users/{userId}", async ([FromRoute] string userId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUserRequest(userId))));
            endpoints.MapPut("/api/users/{userId}", async ([FromRoute] string userId, [FromBody] UserDto user, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUserRequest(userId, user))));
            endpoints.MapPut("/api/users/{userId}/role", async ([FromRoute] string userId, [FromBody] string roleId, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUserRoleRequest(userId, roleId))));
            return endpoints;
        }
    }
}
