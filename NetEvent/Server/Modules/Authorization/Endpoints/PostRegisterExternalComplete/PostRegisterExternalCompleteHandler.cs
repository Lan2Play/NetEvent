using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterExternalCompleteHandler : IRequestHandler<PostRegisterExternalCompleteRequest, PostRegisterExternalCompleteResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<PostRegisterExternalCompleteHandler> _Logger;

        public PostRegisterExternalCompleteHandler(UserManager<ApplicationUser> userManager, ILogger<PostRegisterExternalCompleteHandler> logger)
        {
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<PostRegisterExternalCompleteResponse> Handle(PostRegisterExternalCompleteRequest request, CancellationToken cancellationToken)
        {
            var completeRegistrationRequest = request.CompleteRegistrationRequest;

            var user = await _UserManager.FindByIdAsync(completeRegistrationRequest.UserId);

            if (user == null)
            {
                _Logger.LogError("User {UserId} not found.", completeRegistrationRequest.UserId);
                return new PostRegisterExternalCompleteResponse(ReturnType.Error, $"User {completeRegistrationRequest.UserId} not found.");
            }

            user.FirstName = completeRegistrationRequest.FirstName;
            user.LastName = completeRegistrationRequest.LastName;
            user.Email = completeRegistrationRequest.Email;

            var result = await _UserManager.UpdateAsync(user).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                var errors = string.Join(',', result.Errors.Select(a => a.Description));

                _Logger.LogError("User {UserId} couldn't be updated. Errors: {IdentityErrors}", completeRegistrationRequest.UserId, errors);

                return new PostRegisterExternalCompleteResponse(ReturnType.Error, $"User {completeRegistrationRequest.UserId} couldn't be updated.");
            }

            return new PostRegisterExternalCompleteResponse();
        }
    }
}
