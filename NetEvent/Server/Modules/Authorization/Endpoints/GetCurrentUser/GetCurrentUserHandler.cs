using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
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
                var errorMessage = "User not found.";
                _Logger.LogError(errorMessage);
                return new GetCurrentUserResponse(ReturnType.Error, errorMessage);
            }

            var currentUser = DtoMapper.Mapper.ClaimsPrincipalToCurrentUserDto(request.User);
            currentUser.Claims = request.User.Claims.ToDictionary(c => c.Type, c => c.Value);
            currentUser.Id = request.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return new GetCurrentUserResponse(currentUser);
        }
    }
}
