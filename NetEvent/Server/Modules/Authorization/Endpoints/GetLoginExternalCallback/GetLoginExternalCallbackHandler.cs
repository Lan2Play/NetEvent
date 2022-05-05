using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternalCallback
{
    public class GetLoginExternalCallbackHandler : IRequestHandler<GetLoginExternalCallbackRequest, GetLoginExternalCallbackResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<GetLoginExternalCallbackHandler> _Logger;

        public GetLoginExternalCallbackHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<GetLoginExternalCallbackHandler> logger)
        {
            _SignInManager = signInManager;
            _UserManager = userManager;
            _Logger = logger;
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
            
            if(externalLoginResult.IsNotAllowed)
            {
                // TODO Redirect to not allowed site --> E-Mail erneut senden, E-Mail noch nicht bestätigt
            }

            var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
            var userId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return new GetLoginExternalCallbackResponse(ReturnType.Error, $"No claim found for {ClaimTypes.NameIdentifier}");
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = userId.Split('/').Last();
            }

            var user = new ApplicationUser { UserName = userName, Id = userId, PasswordHash = string.Empty };

            _UserManager.UserValidators.Clear();

            var result = await _UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _UserManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _SignInManager.SignInAsync(user, isPersistent: false);
                    return new GetLoginExternalCallbackResponse(Results.LocalRedirect(returnUrl));
                }
            }

            return new GetLoginExternalCallbackResponse(Results.BadRequest());
        }
    }
}
