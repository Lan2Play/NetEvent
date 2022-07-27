using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NetEvent.Server.Modules.Authorization.Endpoints;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization
{
    [ExcludeFromCodeCoverage]
    public class AuthModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/auth/user", async (ClaimsPrincipal user, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetCurrentUser.Request(user)))).RequireAuthorization();
            endpoints.MapPost("/api/auth/login", async ([FromBody] LoginRequestDto loginRequestDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostLogin.Request(loginRequestDto))));
            endpoints.MapGet("/api/auth/login/external/{provider}", async ([FromRoute] string provider, [FromQuery] string returnUrl, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetLoginExternal.Request(provider, returnUrl))));
            endpoints.MapGet("/api/auth/login/external/{provider}/callback", async ([FromQuery] string returnUrl, [FromQuery] string? remoteError, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetLoginExternalCallback.Request(returnUrl))));
            endpoints.MapPost("/api/auth/logout", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new PostLogoutUser.Request()))).RequireAuthorization();
            endpoints.MapPost("/api/auth/register", async ([FromBody] RegisterRequestDto registerRequestDto, HttpContext httpContext, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRegisterUser.Request(registerRequestDto, httpContext))));
            endpoints.MapPost("/api/auth/register/external/complete", async ([FromBody] RegisterExternalCompleteRequestDto registerCompleteRequestDto, HttpContext httpContext, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostRegisterExternalComplete.Request(registerCompleteRequestDto, httpContext))));
            return endpoints;
        }
    }
}
