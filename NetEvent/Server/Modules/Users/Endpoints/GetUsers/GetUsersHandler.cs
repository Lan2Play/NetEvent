using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;

        public GetUsersHandler(ApplicationDbContext userDbContext)
        {
            _UserDbContext = userDbContext;
        }

        public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var allUsers = await _UserDbContext.Users.ToListAsync();
            return new GetUsersResponse(allUsers);
        }
    }
}
