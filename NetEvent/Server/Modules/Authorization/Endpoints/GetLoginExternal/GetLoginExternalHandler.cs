using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternal
{
    public class GetLoginExternalHandler : IRequestHandler<GetLoginExternalRequest, GetLoginExternalResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<GetLoginExternalHandler> _Logger;

        public GetLoginExternalHandler(SignInManager<ApplicationUser> signInManager, ILogger<GetLoginExternalHandler> logger)
        {
            _SignInManager = signInManager;
            _Logger = logger;
        }

        public async Task<GetLoginExternalResponse> Handle(GetLoginExternalRequest request, CancellationToken cancellationToken)
        {
            var provider = request.Provider;

            var returnUrl = request.ReturnUrl;

            var encodedReturnUrl = HttpUtility.UrlEncode(returnUrl);

            var properties = _SignInManager.ConfigureExternalAuthenticationProperties(provider, $"/api/auth/login/external/callback?returnUrl={encodedReturnUrl}");
            return new GetLoginExternalResponse(Results.Challenge(properties, new[] { provider }));
        }
    }
}
