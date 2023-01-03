using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using NetEvent.Server.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class PostRegisterUser
    {
        public sealed class Handler : AuthRegisterHandlerBase, IRequestHandler<Request, Response>
        {
            private readonly UserManager<ApplicationUser> _UserManager;
            private readonly RoleManager<ApplicationRole> _RoleManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IEmailService emailService, ILogger<Handler> logger) : base(userManager, emailService)
            {
                _UserManager = userManager;
                _RoleManager = roleManager;
                _Logger = logger;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request?.RegisterRequest == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                return InternalHandle(request, cancellationToken);
            }

            private async Task<Response> InternalHandle(Request request, CancellationToken cancellationToken)
            {
                var context = request.HttpContext;

                var user = new ApplicationUser
                {
                    EmailConfirmed = false,
                    UserName = request.RegisterRequest!.Email,
                    Email = request.RegisterRequest!.Email,
                    FirstName = request.RegisterRequest!.FirstName,
                    LastName = request.RegisterRequest!.LastName,
                };

                var result = await _UserManager.CreateAsync(user, request.RegisterRequest!.Password);

                if (result.Succeeded)
                {
                    var defaultRole = await _RoleManager.Roles.FirstAsync(r => r.IsDefault, cancellationToken);
                    result = await _UserManager.AddToRoleAsync(user, defaultRole.Name!);

                    if (result.Succeeded)
                    {
                        await SendConfirmEmailAsync(user, context, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                {
                    _Logger.LogError("Errors occured registering user. Errors: {IdentityErrors}", string.Join(',', result.Errors.Select(a => a.Description)));
                    return new Response(ReturnType.Error, "Error registering user.");
                }

                // Schedule Task for sending E-Mail
                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(RegisterRequestDto registerRequest, HttpContext httpContext)
            {
                RegisterRequest = registerRequest;
                HttpContext = httpContext;
            }

            public RegisterRequestDto RegisterRequest { get; }

            public HttpContext HttpContext { get; }
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
