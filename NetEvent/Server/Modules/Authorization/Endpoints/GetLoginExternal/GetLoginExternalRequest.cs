using MediatR;
using Microsoft.Toolkit.Diagnostics;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternal
{
    public class GetLoginExternalRequest : IRequest<GetLoginExternalResponse>
    {
        public GetLoginExternalRequest(string provider, string returnUrl)
        {
            Guard.IsNotNullOrEmpty(provider, nameof(provider));
            Guard.IsNotNullOrEmpty(returnUrl, nameof(returnUrl));

            Provider = provider;
            ReturnUrl = returnUrl;
        }

        public string Provider { get; }

        public string ReturnUrl { get; }
    }
}
