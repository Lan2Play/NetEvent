using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUser
{
    public class GetUserEmailConfirmHandler : IRequestHandler<GetUserEmailConfirmRequest, GetUserEmailConfirmResponse>
    {
        private const string _ErrorUrl = "/confirmation/error";
        private const string _SuccessUrl = "/confirmation/success";

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<GetUserEmailConfirmHandler> _Logger;

        public GetUserEmailConfirmHandler(UserManager<ApplicationUser> userManager, ILogger<GetUserEmailConfirmHandler> logger)
        {
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<GetUserEmailConfirmResponse> Handle(GetUserEmailConfirmRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var code = request.Code;

            if (userId == null || code == null)
            {
                _Logger.LogError("Not all parameters where supplied.");
                return new GetUserEmailConfirmResponse(Results.LocalRedirect(_ErrorUrl));
            }

            var user = await _UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                _Logger.LogError("Unable to load user with Id {UserId}.", userId);

                return new GetUserEmailConfirmResponse(Results.LocalRedirect(_ErrorUrl));
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _UserManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return new GetUserEmailConfirmResponse(Results.LocalRedirect(_SuccessUrl));
            }
            else
            {
                return new GetUserEmailConfirmResponse(Results.LocalRedirect(_ErrorUrl));
            }
        }
    }
}
