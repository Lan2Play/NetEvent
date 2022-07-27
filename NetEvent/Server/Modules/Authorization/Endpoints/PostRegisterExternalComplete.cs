using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using NetEvent.Server.Services;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public static class PostRegisterExternalComplete
    {
        public class Handler : AuthRegisterHandlerBase, IRequestHandler<Request, Response>
        {
            private readonly UserManager<ApplicationUser> _UserManager;
            private readonly ILogger<Handler> _Logger;

            public Handler(UserManager<ApplicationUser> userManager, IEmailService emailService, ILogger<Handler> logger) : base(userManager, emailService)
            {
                _UserManager = userManager;
                _Logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var completeRegistrationRequest = request.CompleteRegistrationRequest;
                var context = request.HttpContext;

                var user = await _UserManager.FindByIdAsync(completeRegistrationRequest.UserId);

                if (user == null)
                {
                    _Logger.LogError("User {UserId} not found.", completeRegistrationRequest.UserId);
                    return new Response(ReturnType.Error, $"User {completeRegistrationRequest.UserId} not found.");
                }

                user.FirstName = completeRegistrationRequest.FirstName;
                user.LastName = completeRegistrationRequest.LastName;
                user.Email = completeRegistrationRequest.Email;

                var result = await _UserManager.UpdateAsync(user).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    var errors = string.Join(',', result.Errors.Select(a => a.Description));

                    _Logger.LogError("User {UserId} couldn't be updated. Errors: {IdentityErrors}", completeRegistrationRequest.UserId, errors);

                    return new Response(ReturnType.Error, $"User {completeRegistrationRequest.UserId} couldn't be updated.");
                }

                await SendConfirmEmailAsync(user, context, cancellationToken).ConfigureAwait(false);

                return new Response();
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(RegisterExternalCompleteRequestDto completeRegistrationRequest, HttpContext httpContext)
            {
                CompleteRegistrationRequest = completeRegistrationRequest;
                HttpContext = httpContext;
            }

            public RegisterExternalCompleteRequestDto CompleteRegistrationRequest { get; }

            public HttpContext HttpContext { get; }
        }

        public class Response : ResponseBase
        {
            public Response()
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
