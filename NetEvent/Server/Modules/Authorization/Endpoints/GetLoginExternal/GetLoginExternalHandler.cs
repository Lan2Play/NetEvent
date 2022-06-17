using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternal
{
    public class GetLoginExternalHandler : IRequestHandler<GetLoginExternalRequest, GetLoginExternalResponse>
    {
        private readonly SignInManager<ApplicationUser> _SignInManager;

        public GetLoginExternalHandler(SignInManager<ApplicationUser> signInManager)
        {
            _SignInManager = signInManager;
        }

        public Task<GetLoginExternalResponse> Handle(GetLoginExternalRequest request, CancellationToken cancellationToken)
        {
            var provider = request.Provider;

            var returnUrl = request.ReturnUrl;

            var encodedReturnUrl = HttpUtility.UrlEncode(returnUrl);

            var properties = _SignInManager.ConfigureExternalAuthenticationProperties(provider, $"/api/auth/login/external/{provider}/callback?returnUrl={encodedReturnUrl}");
            return Task.FromResult(new GetLoginExternalResponse(Results.Challenge(properties, new[] { provider })));
        }
    }
}
