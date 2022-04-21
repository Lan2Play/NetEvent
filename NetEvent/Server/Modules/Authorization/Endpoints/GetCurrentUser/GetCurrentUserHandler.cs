using MediatR;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
    {
        private readonly ILogger<GetCurrentUserHandler> _Logger;

        public GetCurrentUserHandler(ILogger<GetCurrentUserHandler> logger)
        {
            _Logger = logger;
        }

        public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
        {
            var claimsPrincipal = request.User;

            if (claimsPrincipal == null)
            {
                return new GetCurrentUserResponse(ReturnType.Error, "User not found.");
            }

            var currentUser = new CurrentUser
            {
                IsAuthenticated = claimsPrincipal.Identity.IsAuthenticated,
                UserName = claimsPrincipal.Identity.Name,
                Claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value)
            };

            return new GetCurrentUserResponse(currentUser);
        }
    }
}
