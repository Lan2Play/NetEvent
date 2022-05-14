using MediatR;
using Microsoft.Toolkit.Diagnostics;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserEmailConfirmRequest : IRequest<GetUserEmailConfirmResponse>
    {
        public GetUserEmailConfirmRequest(string userId, string code)
        {
            Guard.IsNotNullOrEmpty(userId, nameof(userId));
            Guard.IsNotNullOrEmpty(code, nameof(code));

            UserId = userId;
            Code = code;
        }

        public string UserId { get; }

        public string Code { get; }
    }
}
