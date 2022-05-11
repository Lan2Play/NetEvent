using MediatR;
using Microsoft.Toolkit.Diagnostics;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternalCallback
{
    public class GetLoginExternalCallbackRequest : IRequest<GetLoginExternalCallbackResponse>
    {
        public GetLoginExternalCallbackRequest(string returnUrl)
        {
            Guard.IsNotNullOrEmpty(returnUrl, nameof(returnUrl));

            ReturnUrl = returnUrl;
        }

        public string ReturnUrl { get; }
    }
}
