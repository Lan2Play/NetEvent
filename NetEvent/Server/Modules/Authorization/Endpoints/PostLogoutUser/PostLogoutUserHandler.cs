using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser
{
    public class PostLogoutUserHandler : IRequestHandler<PostLogoutUserRequest, PostLogoutUserResponse>
    {
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<PostLogoutUserHandler> _Logger;

        public PostLogoutUserHandler(SignInManager<ApplicationUser> signInManager, ILogger<PostLogoutUserHandler> logger)
        {
            ;
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public async Task<PostLogoutUserResponse> Handle(PostLogoutUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _SignInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                var errorMessage = "Exception occured on sign out.";
                _Logger.LogError(ex, errorMessage);
                return new PostLogoutUserResponse(ReturnType.Error, errorMessage);
            }

            return new PostLogoutUserResponse();
        }
    }
}
