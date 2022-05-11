using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser;
using NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternal;
using NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternalCallback;
using NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser;
using NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser;
using NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization
{
    [ExcludeFromCodeCoverage]
    public class AuthModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/auth/user", async (ClaimsPrincipal user, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetUserRequest(user)))).RequireAuthorization();
            endpoints.MapPost("/api/auth/login", async ([FromBody] LoginRequestDto loginRequestDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostLoginRequest(loginRequestDto))));
            endpoints.MapGet("/api/auth/login/external/{provider}", async ([FromRoute] string provider, [FromQuery] string returnUrl, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetLoginExternalRequest(provider, returnUrl))));
            endpoints.MapGet("/api/auth/login/external/{provider}/callback", async ([FromQuery] string returnUrl, [FromQuery] string? remoteError, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetLoginExternalCallbackRequest(returnUrl))));
            endpoints.MapPost("/api/auth/logout", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new PostLogoutRequest()))).RequireAuthorization();
            endpoints.MapPost("/api/auth/register", async ([FromBody] RegisterRequestDto registerRequestDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRegisterRequest(registerRequestDto))));
            endpoints.MapPost("/api/auth/register/external/complete", async ([FromBody] RegisterExternalCompleteRequestDto registerCompleteRequestDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRegisterExternalCompleteRequest(registerCompleteRequestDto))));
            return endpoints;
        }
    }
}
