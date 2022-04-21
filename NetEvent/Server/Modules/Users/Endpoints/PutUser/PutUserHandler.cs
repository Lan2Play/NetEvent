using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.PutUser
{
    public class PutUserHandler : IRequestHandler<PutUserRequest, PutUserResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;
        private readonly ILogger<PutUserHandler> _Logger;

        public PutUserHandler(ApplicationDbContext userDbContext, ILogger<PutUserHandler> logger)
        {
            _UserDbContext = userDbContext;
            _Logger = logger;
        }

        public async Task<PutUserResponse> Handle(PutUserRequest request, CancellationToken cancellationToken)
        {
            var oldUser = _UserDbContext.Find<ApplicationUser>(request.Id);

            if (oldUser == null)
            {
                return null;
            }

            UpdateOldUser(oldUser, request.User);
            _UserDbContext.Update(oldUser);
            await _UserDbContext.SaveChangesAsync();

            return new PutUserResponse();
        }

        private static void UpdateOldUser(ApplicationUser oldUser, CurrentUser user)
        {
            oldUser.UserName = user.UserName;
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.EmailConfirmed = user.EmailConfirmed;
        }


    }
}
