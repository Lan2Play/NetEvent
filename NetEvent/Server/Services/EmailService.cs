using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _EmailSender;
        private readonly IEmailRenderer _EmailRenderer;
        private readonly ILogger<EmailService> _Logger;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public EmailService(ApplicationDbContext applicationDbContext, IEmailSender emailSender, IEmailRenderer emailRenderer, ILogger<EmailService> logger)
        {
            _ApplicationDbContext = applicationDbContext;
            _EmailSender = emailSender;
            _EmailRenderer = emailRenderer;
            _Logger = logger;
        }

        public async Task SendMailAsync(string templateId, string receipient, NetEventEmailRenderModel subjectModel, NetEventEmailRenderModel templateModel, CancellationToken cancellationToken)
        {
            // Find Email template
            var emailTemplate = await _ApplicationDbContext.EmailTemplates.FindAsync(new[] { templateId }, cancellationToken).ConfigureAwait(false);

            if (emailTemplate == null)
            {
                _Logger.LogError("E-Mail template {TemplateId} wasn't found.", templateId);
                return;
            }

            var subjectRenderResult = await _EmailRenderer.RenderAsync($"{templateId}_Subject", emailTemplate.SubjectTemplate, subjectModel, cancellationToken).ConfigureAwait(false);

            var contentRenderResult = await _EmailRenderer.RenderAsync($"{templateId}_Content", emailTemplate.ContentTemplate, templateModel, cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(subjectRenderResult))
            {
                _Logger.LogError("E-Mail subject of {TemplateId} wasn't rendered correctly.", templateId);
                return;
            }

            if (string.IsNullOrEmpty(contentRenderResult))
            {
                _Logger.LogError("E-Mail message content of {TemplateId} wasn't rendered correctly.", templateId);
                return;
            }

            await _EmailSender.SendEmailAsync(receipient, subjectRenderResult, contentRenderResult, cancellationToken).ConfigureAwait(false);
        }
    }
}
