using MediatR;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class PutThemeHandler : IRequestHandler<PutThemeRequest, PutThemeResponse>
    {
        private readonly ILogger<PutThemeHandler> _Logger;

        public PutThemeHandler(ILogger<PutThemeHandler> logger)
        {
            _Logger = logger;
        }

        public async Task<PutThemeResponse> Handle(PutThemeRequest request, CancellationToken cancellationToken)
        {
            // TODO Implement
            return new PutThemeResponse();
        }
    }
}
