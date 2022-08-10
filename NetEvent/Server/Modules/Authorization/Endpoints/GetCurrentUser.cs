using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Helpers;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class GetCurrentUser
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly NetEventUserManager _UserManager;
            private readonly SignInManager<ApplicationUser> _SignInManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(NetEventUserManager userManager, SignInManager<ApplicationUser> signInManager, ILogger<Handler> logger)
            {
                _UserManager = userManager;
                _SignInManager = signInManager;
                _Logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.User == null)
                {
                    const string errorMessage = "User not found.";
                    _Logger.LogError(errorMessage);
                    return new Response(ReturnType.Error, errorMessage);
                }

                var userId = request.User.Id();
                var user = await _UserManager.FindByIdAsync(userId);

                var refreshedUser = await _SignInManager.CreateUserPrincipalAsync(user);

                var currentUser = refreshedUser.ToCurrentUserDto();
                currentUser.Claims = refreshedUser.Claims.ToDictionary(c => c.Type, c => c.Value);
                return new Response(currentUser);
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(ClaimsPrincipal user)
            {
                User = user;
            }

            public ClaimsPrincipal User { get; }
        }

        public class Response : ResponseBase<CurrentUserDto>
        {
            public Response(CurrentUserDto? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
