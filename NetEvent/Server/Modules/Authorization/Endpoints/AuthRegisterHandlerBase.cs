using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NetEvent.Server.Models;
using NetEvent.Server.Services;

namespace NetEvent.Server.Modules.Authorization.Endpoints
{
    public class AuthRegisterHandlerBase
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEmailService _EmailService;

        public AuthRegisterHandlerBase(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _UserManager = userManager;
            _EmailService = emailService;
        }

        protected async Task<bool> SendConfirmEmailAsync(ApplicationUser user, HttpContext context, CancellationToken cancellationToken)
        {
            var encodedUrl = context.Request.GetEncodedUrl();

            var uriBuilder = new UriBuilder(encodedUrl)
            {
                Path = string.Empty,
                Query = string.Empty
            };

            var baseUri = uriBuilder.ToString();

            var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var url = $"{baseUri}api/users/{user.Id}/email/confirm?code={encodedCode}";

            var subjectModel = new NetEventEmailRenderModel(new Dictionary<string, string>()
            {
                { "firstName", user.FirstName ?? string.Empty }
            });

            var contentModel = new NetEventEmailRenderModel(new Dictionary<string, string>()
            {
                { "firstName", user.FirstName ?? string.Empty },
                { "confirmUrl", url }
            });

            await _EmailService.SendMailAsync("UserEmailConfirmEmailTemplate", user.Email, subjectModel, contentModel, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}
