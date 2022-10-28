using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class GetLoginExternal
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly SignInManager<ApplicationUser> _SignInManager;

            public Handler(SignInManager<ApplicationUser> signInManager)
            {
                _SignInManager = signInManager;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var provider = request.Provider;

                var returnUrl = request.ReturnUrl;

                var encodedReturnUrl = HttpUtility.UrlEncode(returnUrl);

                var properties = _SignInManager.ConfigureExternalAuthenticationProperties(provider, $"/api/auth/login/external/{provider}/callback?returnUrl={encodedReturnUrl}");
                return Task.FromResult(new Response(Results.Challenge(properties, new[] { provider })));
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(string provider, string returnUrl)
            {
                Guard.IsNotNullOrEmpty(provider, nameof(provider));
                Guard.IsNotNullOrEmpty(returnUrl, nameof(returnUrl));

                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; }

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
