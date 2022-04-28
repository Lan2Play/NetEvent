using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules.Users.Endpoints.GetUser;
using NetEvent.Server.Modules.Users.Endpoints.GetUsers;
using NetEvent.Server.Modules.Users.Endpoints.PutUser;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users
{
    [ExcludeFromCodeCoverage]
    public class UsersModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            //endpoints.MapGet("/users", GetUsers.Handle);
            endpoints.MapGet("/api/users", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetUsersRequest())));
            endpoints.MapGet("/api/users/{id}", async ([FromRoute] string id, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUserRequest(id))));
            endpoints.MapPut("/api/users/{id}", async ([FromRoute] string id, [FromBody] UserDto user, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUserRequest(id, user))));
            return endpoints;
        }

        public override IServiceCollection RegisterModule(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(UsersModule));

            return builder;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
