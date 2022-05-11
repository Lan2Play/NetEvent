using System.Linq;
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
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
    {
        private readonly NetEventUserManager _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<GetCurrentUserHandler> _Logger;

        public GetCurrentUserHandler(NetEventUserManager userManager, SignInManager<ApplicationUser> signInManager, ILogger<GetCurrentUserHandler> logger)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                var errorMessage = "User not found.";
                _Logger.LogError(errorMessage);
                return new GetCurrentUserResponse(ReturnType.Error, errorMessage);
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
