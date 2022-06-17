using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser
{
    public class PostLogoutHandler : IRequestHandler<PostLogoutRequest, PostLogoutResponse>
    {
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<PostLogoutHandler> _Logger;

        public PostLogoutHandler(SignInManager<ApplicationUser> signInManager, ILogger<PostLogoutHandler> logger)
        {
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public async Task<PostLogoutResponse> Handle(PostLogoutRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _SignInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                const string errorMessage = "Exception occured on sign out.";
                _Logger.LogError(ex, errorMessage);
                return new PostLogoutResponse(ReturnType.Error, errorMessage);
            }

            return new PostLogoutResponse();
        }
    }
}
