using MediatR;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetThemeHandler : IRequestHandler<GetThemeRequest, GetThemeResponse>
    {
        private readonly ILogger<GetThemeHandler> _Logger;

        public GetThemeHandler(ILogger<GetThemeHandler> logger)
        {
            _Logger = logger;
        }

        public async Task<GetThemeResponse> Handle(GetThemeRequest request, CancellationToken cancellationToken)
        {
            // TODO Implement
            return new GetThemeResponse(null);
        }
    }
}
