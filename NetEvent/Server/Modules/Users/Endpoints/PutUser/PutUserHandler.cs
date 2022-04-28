using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

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
            var user = request.User;
            
            var existingUser = await _UserDbContext.FindAsync<ApplicationUser>(request.Id).ConfigureAwait(false);

            if (existingUser == null)
            {
                return new PutUserResponse(ReturnType.NotFound, $"User {request.Id} not found in database.");
            }

            // TODO Validate new user

            // Update existing user
            existingUser.UserName = user.UserName;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.EmailConfirmed = user.EmailConfirmed;

            _UserDbContext.Update(existingUser);
            await _UserDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new PutUserResponse();
        }
    }
}
