using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

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

            var currentUser = new CurrentUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                ProfileImage = user.ProfilePicture,
                EmailConfirmed = user.EmailConfirmed,   
                
            };

            return new GetUserResponse(currentUser);
        }
    }
}
