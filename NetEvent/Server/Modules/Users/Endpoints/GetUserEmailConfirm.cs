using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using NetEvent.Shared;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public static class GetUserEmailConfirm
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private const string _ErrorUrl = "/confirmation/error";
            private const string _SuccessUrl = "/confirmation/success";

            private readonly UserManager<ApplicationUser> _UserManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(UserManager<ApplicationUser> userManager, ILogger<Handler> logger)
            {
                _UserManager = userManager;
                _Logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = request.UserId;
                var code = request.Code;

                if (userId == null || code == null)
                {
                    _Logger.LogError("Not all parameters where supplied.");
                    return new Response(Results.LocalRedirect(_ErrorUrl));
                }

                var user = await _UserManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _Logger.LogError("Unable to load user with Id {UserId}.", userId);

                    return new Response(Results.LocalRedirect(_ErrorUrl));
                }

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _UserManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    return new Response(Results.LocalRedirect(_SuccessUrl));
                }
                else
                {
                    return new Response(Results.LocalRedirect(_ErrorUrl));
                }
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(string userId, string code)
            {
                Guard.IsNotNullOrEmpty(userId, nameof(userId));
                Guard.IsNotNullOrEmpty(code, nameof(code));

                UserId = userId;
                Code = code;
            }

            public string UserId { get; }

            public string Code { get; }
        }

        public sealed class Response : ResponseBase<IResult>
        {
            public Response(IResult result) : base(result)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
