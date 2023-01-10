using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class PostLogin
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly UserManager<ApplicationUser> _UserManager;
            private readonly SignInManager<ApplicationUser> _SignInManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<Handler> logger)
            {
                _UserManager = userManager;
                _SignInManager = signInManager;
                _Logger = logger;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request?.LoginRequest == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(request));
                }

                return InternalHandle(request);
            }

            private async Task<Response> InternalHandle(Request request)
            {
                var user = (await _UserManager.FindByNameAsync(request.LoginRequest!.UserName).ConfigureAwait(false)) ??
                                        await _UserManager.FindByEmailAsync(request.LoginRequest!.UserName).ConfigureAwait(false);

                if (user == null)
                {
                    const string errorMessage = "User does not exist.";
                    _Logger.LogError(errorMessage);
                    return new Response(ReturnType.Error, errorMessage);
                }

                var singInResult = await _SignInManager.CheckPasswordSignInAsync(user, request.LoginRequest!.Password, false);

                if (!singInResult.Succeeded)
                {
                    const string errorMessage = "Invalid password.";
                    _Logger.LogError(errorMessage);
                    return new Response(ReturnType.Error, errorMessage);
                }

                await _SignInManager.SignInAsync(user, request.LoginRequest!.RememberMe).ConfigureAwait(false);
                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(LoginRequestDto loginRequest)
            {
                LoginRequest = loginRequest;
            }

            public LoginRequestDto LoginRequest { get; }
        }

        public sealed class Response : ResponseBase
        {
            public Response()
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
