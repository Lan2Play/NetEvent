using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class GetLoginExternalCallback
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly SignInManager<ApplicationUser> _SignInManager;
            private readonly UserManager<ApplicationUser> _UserManager;
            private readonly RoleManager<ApplicationRole> _RoleManager;

            public Handler(SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
            {
                _SignInManager = signInManager;
                _UserManager = userManager;
                _RoleManager = roleManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var returnUrl = request.ReturnUrl;

                var info = await _SignInManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return new Response(ReturnType.Error, $"Error loading external login information.");
                }

                var externalLoginResult = await _SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                if (externalLoginResult.Succeeded)
                {
                    var existingUser = await _UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey).ConfigureAwait(false);
                    if (existingUser == null)
                    {
                        const string errorMessage = "Existing user not found.";
                        return new Response(ReturnType.Error, errorMessage);
                    }

                    if (existingUser.EmailConfirmed)
                    {
                        return new Response(Results.LocalRedirect("/"));
                    }

                    return new Response(Results.LocalRedirect(returnUrl));
                }

                if (externalLoginResult.IsNotAllowed)
                {
                    return new Response(Results.LocalRedirect("/confirmation/pending"));
                }

                var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
                var steamUserId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(steamUserId))
                {
                    return new Response(ReturnType.Error, $"No claim found for {ClaimTypes.NameIdentifier}");
                }

                if (string.IsNullOrEmpty(userName))
                {
                    userName = steamUserId.Split('/').Last();
                }

                var user = new ApplicationUser { UserName = userName, PasswordHash = string.Empty };

                var result = await _UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    var defaultRole = await _RoleManager.Roles.FirstAsync(x => x.IsDefault, cancellationToken);

                    if (string.IsNullOrEmpty(defaultRole.Name))
                    {
                        return new Response(ReturnType.Error, $"No default Role found!");
                    }


                    result = await _UserManager.AddToRoleAsync(user, defaultRole.Name).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        result = await _UserManager.AddLoginAsync(user, info);

                        if (result.Succeeded)
                        {
                            await _SignInManager.SignInAsync(user, isPersistent: false);
                            return new Response(Results.LocalRedirect(returnUrl));
                        }
                    }
                }

                return new Response(Results.BadRequest());
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string returnUrl)
            {
                Guard.IsNotNullOrEmpty(returnUrl, nameof(returnUrl));

                ReturnUrl = returnUrl;
            }

            public string ReturnUrl { get; }
        }

        public class Response : ResponseBase<IResult>
        {
            public Response(IResult value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
