using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;

        public GetUserHandler(ApplicationDbContext userDbContext)
        {
            _UserDbContext = userDbContext;
        }

        public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _UserDbContext.FindAsync<ApplicationUser>(request.Id);
            if (user == null)
            {
                return new GetUserResponse(ReturnType.NotFound, string.Empty);
            }

            var currentUser = DtoMapper.Mapper.ApplicaitonUserToUserDto(user);

            return new GetUserResponse(currentUser);
        }
    }
}
