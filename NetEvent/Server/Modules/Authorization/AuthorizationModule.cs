using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser;
using NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser;
using NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser;
using NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;
using NetEvent.Shared.Dto;
using System.Security.Claims;

namespace NetEvent.Server.Modules.Authorization
{
    public class AuthorizationModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/auth/register", async ([FromBody] RegisterRequest registerRequest, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRegisterUserRequest(registerRequest))));
            endpoints.MapPost("/api/auth/login", async ([FromBody] LoginRequest loginRequest, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostLoginUserRequest(loginRequest))));
            endpoints.MapPost("/api/auth/logout", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new PostLogoutUserRequest()))).RequireAuthorization();
            endpoints.MapGet("/api/auth/user/current", async (ClaimsPrincipal user, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetCurrentUserRequest(user)))).RequireAuthorization();

            return endpoints;
        }

        public override IServiceCollection RegisterModule(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(AuthorizationModule));

            return builder;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
