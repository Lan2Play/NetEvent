using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class PostLogoutUser
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly SignInManager<ApplicationUser> _SignInManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(SignInManager<ApplicationUser> signInManager, ILogger<Handler> logger)
            {
                _SignInManager = signInManager;
                _Logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    await _SignInManager.SignOutAsync();
                }
                catch (Exception ex)
                {
                    const string errorMessage = "Exception occured on sign out.";
                    _Logger.LogError(ex, errorMessage);
                    return new Response(ReturnType.Error, errorMessage);
                }

                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
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
