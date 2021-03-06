using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Users.Endpoints.PutUser
{
    public class PutUserHandler : IRequestHandler<PutUserRequest, PutUserResponse>
    {
        private readonly NetEventUserManager _UserManager;

        public PutUserHandler(NetEventUserManager userManager)
        {
            _UserManager = userManager;
        }

        public async Task<PutUserResponse> Handle(PutUserRequest request, CancellationToken cancellationToken)
        {
            var user = request.User;

            var existingUser = await _UserManager.FindByIdAsync(request.Id).ConfigureAwait(false);

            if (existingUser == null)
            {
                return new PutUserResponse(ReturnType.NotFound, $"User {request.Id} not found in database.");
            }

            // Update existing user
            existingUser.UserName = user.UserName;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.EmailConfirmed = user.EmailConfirmed;

            await _UserManager.UpdateAsync(existingUser);

            return new PutUserResponse();
        }
    }
}
