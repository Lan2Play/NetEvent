using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using NetEvent.Server.Services;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterHandler : AuthRegisterHandlerBase, IRequestHandler<PostRegisterRequest, PostRegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly ILogger<PostRegisterHandler> _Logger;

        public PostRegisterHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, ILogger<PostRegisterHandler> logger) : base(userManager, emailService)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _Logger = logger;
        }

        public async Task<PostRegisterResponse> Handle(PostRegisterRequest request, CancellationToken cancellationToken)
        {
            var context = request.HttpContext;

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
                result = await _UserManager.AddToRoleAsync(user, defaultRole.Name);

                if (result.Succeeded)
                {
                    var emailSent = await SendConfirmEmailAsync(user, context, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                _Logger.LogError("Errors occured registering user. Errors: {IdentityErrors}", string.Join(',', result.Errors.Select(a => a.Description)));
                return new PostRegisterResponse(ReturnType.Error, "Error registering user.");
            }

            // Schedule Task for sending E-Mail
            return new PostRegisterResponse();
        }
    }
}
