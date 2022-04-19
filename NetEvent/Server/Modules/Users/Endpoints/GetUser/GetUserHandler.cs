using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users.Endpoints
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
                return new GetUserResponse(ReturnType.NotFound, "");
            }

            return new GetUserResponse(user);
        }
    }
}
