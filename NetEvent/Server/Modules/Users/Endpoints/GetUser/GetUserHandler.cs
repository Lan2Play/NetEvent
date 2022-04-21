using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;

        private readonly ILogger<GetUserHandler> _Logger;

        public GetUserHandler(ApplicationDbContext userDbContext, ILogger<GetUserHandler> logger)
        {
            _UserDbContext = userDbContext;
            _Logger = logger;
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
