using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostCompleteRegistrationHandler : IRequestHandler<PostCompleteRegistrationRequest, PostCompleteRegistrationResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<PostCompleteRegistrationHandler> _Logger;

        public PostCompleteRegistrationHandler(UserManager<ApplicationUser> userManager, ILogger<PostCompleteRegistrationHandler> logger)
        {
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<PostCompleteRegistrationResponse> Handle(PostCompleteRegistrationRequest request, CancellationToken cancellationToken)
        {
            var completeRegistrationRequest = request.CompleteRegistrationRequest;

            var user = await _UserManager.FindByIdAsync(completeRegistrationRequest.UserId);

            if (user == null)
            {
                var errorMessage = $"User {completeRegistrationRequest.UserId} not found.";
                _Logger.LogError(errorMessage);
                return new PostCompleteRegistrationResponse(ReturnType.Error, errorMessage);
            }

            user.FirstName = completeRegistrationRequest.FirstName;
            user.LastName = completeRegistrationRequest.LastName;
            user.Email = completeRegistrationRequest.Email;
            //user.UserName = completeRegistrationRequest.Email;

            await _UserManager.UpdateAsync(user).ConfigureAwait(false);

            return new PostCompleteRegistrationResponse();
        }
    }
}
