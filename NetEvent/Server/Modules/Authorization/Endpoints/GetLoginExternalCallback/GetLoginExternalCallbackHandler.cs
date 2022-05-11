using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternalCallback
{
    public class GetLoginExternalCallbackHandler : IRequestHandler<GetLoginExternalCallbackRequest, GetLoginExternalCallbackResponse>
    {
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public GetLoginExternalCallbackHandler(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _SignInManager = signInManager;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public async Task<GetLoginExternalCallbackResponse> Handle(GetLoginExternalCallbackRequest request, CancellationToken cancellationToken)
        {
            var returnUrl = request.ReturnUrl;

            var info = await _SignInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return new GetLoginExternalCallbackResponse(ReturnType.Error, $"Error loading external login information.");
            }

            var externalLoginResult = await _SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (externalLoginResult.Succeeded)
            {
                return new GetLoginExternalCallbackResponse(Results.LocalRedirect(returnUrl));
            }

            if (externalLoginResult.IsNotAllowed)
            {
                return new GetLoginExternalCallbackResponse(Results.LocalRedirect("/confirmation/pending"));
            }

            var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
            var steamUserId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(steamUserId))
            {
                return new GetLoginExternalCallbackResponse(ReturnType.Error, $"No claim found for {ClaimTypes.NameIdentifier}");
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = steamUserId.Split('/').Last();
            }

            var user = new ApplicationUser { UserName = userName, PasswordHash = string.Empty };

            var result = await _UserManager.CreateAsync(user);

            if (result.Succeeded)
            {
                var defaultRole = await _RoleManager.Roles.FirstAsync(cancellationToken);

                result = await _UserManager.AddToRoleAsync(user, defaultRole.Name).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    result = await _UserManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        await _SignInManager.SignInAsync(user, isPersistent: false);
                        return new GetLoginExternalCallbackResponse(Results.LocalRedirect(returnUrl));
                    }
                }
            }

            return new GetLoginExternalCallbackResponse(Results.BadRequest());
        }
    }
}
