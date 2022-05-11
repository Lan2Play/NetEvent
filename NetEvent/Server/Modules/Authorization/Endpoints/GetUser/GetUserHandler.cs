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

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        private readonly NetEventUserManager _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<GetUserHandler> _Logger;

        public GetUserHandler(NetEventUserManager userManager, SignInManager<ApplicationUser> signInManager, ILogger<GetUserHandler> logger)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                var errorMessage = "User not found.";
                _Logger.LogError(errorMessage);
                return Task.FromResult(new GetUserResponse(ReturnType.Error, errorMessage));
            }

            var userId = request.User.Id();
            var user = await _UserManager.FindByIdAsync(userId);

            var refreshedUser = await _SignInManager.CreateUserPrincipalAsync(user);

            var currentUser = DtoMapper.Mapper.ClaimsPrincipalToCurrentUserDto(refreshedUser);
            currentUser.Claims = refreshedUser.Claims.ToDictionary(c => c.Type, c => c.Value);
            return new GetCurrentUserResponse(currentUser);
        }
    }
}
