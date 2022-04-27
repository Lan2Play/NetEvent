using MediatR;
using NetEvent.Shared;

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
            if (request.User == null)
            {
                return new GetCurrentUserResponse(ReturnType.Error, "User not found.");
            }

            var currentUser = DtoMapper.Mapper.ClaimsPrincipalToCurrentUserDto(request.User);
            currentUser.Claims = request.User.Claims.ToDictionary(c => c.Type, c => c.Value);
            return new GetCurrentUserResponse(currentUser);
        }
    }
}
