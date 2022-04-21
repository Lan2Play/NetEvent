using MediatR;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser
{
    public class PostLogoutUserHandler : IRequestHandler<PostLogoutUserRequest, PostLogoutUserResponse>
    {
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<PostLogoutUserHandler> _Logger;

        public PostLogoutUserHandler(SignInManager<ApplicationUser> signInManager, ILogger<PostLogoutUserHandler> logger)
        {;
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public async Task<PostLogoutUserResponse> Handle(PostLogoutUserRequest request, CancellationToken cancellationToken)
        {
            await _SignInManager.SignOutAsync();

            return new PostLogoutUserResponse();
        }
    }
}
