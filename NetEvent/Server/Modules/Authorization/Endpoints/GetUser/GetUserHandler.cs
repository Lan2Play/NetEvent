using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetCurrentUser
{
    public class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        private readonly ILogger<GetUserHandler> _Logger;

        public GetUserHandler(ILogger<GetUserHandler> logger)
        {
            _Logger = logger;
        }

        public Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                var errorMessage = "User not found.";
                _Logger.LogError(errorMessage);
                return Task.FromResult(new GetUserResponse(ReturnType.Error, errorMessage));
            }

            var currentUser = DtoMapper.Mapper.ClaimsPrincipalToCurrentUserDto(request.User);
            currentUser.Claims = request.User.Claims.ToDictionary(c => c.Type, c => c.Value);
            currentUser.Id = request.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return Task.FromResult(new GetUserResponse(currentUser));
        }
    }
}
