using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser
{
    public class PostLoginHandler : IRequestHandler<PostLoginRequest, PostLoginResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<PostLoginHandler> _Logger;

        public PostLoginHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<PostLoginHandler> logger)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public async Task<PostLoginResponse> Handle(PostLoginRequest request, CancellationToken cancellationToken)
        {
            var user = (await _UserManager.FindByNameAsync(request.LoginRequest.UserName).ConfigureAwait(false)) ??
                        await _UserManager.FindByEmailAsync(request.LoginRequest.UserName).ConfigureAwait(false);

            if (user == null)
            {
                var errorMessage = "User does not exist.";
                _Logger.LogError(errorMessage);
                return new PostLoginResponse(ReturnType.Error, errorMessage);
            }

            var singInResult = await _SignInManager.CheckPasswordSignInAsync(user, request.LoginRequest.Password, false);

            if (!singInResult.Succeeded)
            {
                var errorMessage = "Invalid password.";
                _Logger.LogError(errorMessage);
                return new PostLoginResponse(ReturnType.Error, errorMessage);
            }

            await _SignInManager.SignInAsync(user, request.LoginRequest.RememberMe).ConfigureAwait(false);
            return new PostLoginResponse();
        }
    }
}
