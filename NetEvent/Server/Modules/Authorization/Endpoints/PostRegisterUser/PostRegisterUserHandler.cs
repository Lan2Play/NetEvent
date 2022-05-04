using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterUserHandler : IRequestHandler<PostRegisterUserRequest, PostRegisterUserResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<PostRegisterUserHandler> _Logger;

        public PostRegisterUserHandler(UserManager<ApplicationUser> userManager, ILogger<PostRegisterUserHandler> logger)
        {
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<PostRegisterUserResponse> Handle(PostRegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                EmailConfirmed = false,
                UserName = request.RegisterRequest.Email,
                Email = request.RegisterRequest.Email,
                FirstName = request.RegisterRequest.FirstName,
                LastName = request.RegisterRequest.LastName,
            };

            var result = await _UserManager.CreateAsync(user, request.RegisterRequest.Password);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();

                sb.Append("Errors registering user: ");

                foreach (var error in result.Errors)
                {
                    sb.Append(", ");
                    sb.Append(error.Description);
                }

                var errorMessage = sb.ToString();

                _Logger.LogError(errorMessage);
                return new PostRegisterUserResponse(ReturnType.Error, errorMessage);
            }

            // TODO Schedule Task for sending E-Mail
            return new PostRegisterUserResponse();
        }
    }
}
