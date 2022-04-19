using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetEvent.Server.Modules.Users.Endpoints;
using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users
{
    public class UsersModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            //endpoints.MapGet("/users", GetUsers.Handle);
            endpoints.MapGet("/users", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetUsersRequest())));
            endpoints.MapGet("/users/{id}", GetUser.Handle);
            endpoints.MapPut("/users/{id}", async ([FromRoute] string id, [FromBody] ApplicationUser user, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutUserRequest(id, user))));
            return endpoints;
        }

        public override IServiceCollection RegisterModule(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(UsersModule));

            return builder;
        }
    }
}
