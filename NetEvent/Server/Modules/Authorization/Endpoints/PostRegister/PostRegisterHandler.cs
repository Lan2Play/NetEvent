using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterHandler : IRequestHandler<PostRegisterRequest, PostRegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly ILogger<PostRegisterHandler> _Logger;

        public PostRegisterHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<PostRegisterHandler> logger)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _Logger = logger;
        }

        public async Task<PostRegisterResponse> Handle(PostRegisterRequest request, CancellationToken cancellationToken)
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

            if (result.Succeeded)
            {
                var defaultRole = await _RoleManager.Roles.FirstAsync(cancellationToken);
                await _UserManager.AddToRoleAsync(user, defaultRole.Name);
            }
            else
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
                return new PostRegisterResponse(ReturnType.Error, errorMessage);
            }

            // TODO Schedule Task for sending E-Mail
            return new PostRegisterResponse();
        }
    }
}
