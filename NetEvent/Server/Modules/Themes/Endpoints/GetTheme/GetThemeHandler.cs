using MediatR;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Themes.Endpoints.GetTheme
{
    public class GetThemeHandler : IRequestHandler<GetThemeRequest, GetThemeResponse>
    {
        private readonly ILogger<GetThemeHandler> _Logger;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public GetThemeHandler(ApplicationDbContext applicationDbContext, ILogger<GetThemeHandler> logger)
        {
            _ApplicationDbContext = applicationDbContext;
            _Logger = logger;
        }

        public Task<GetThemeResponse> Handle(GetThemeRequest request, CancellationToken cancellationToken)
        {
            var theme = _ApplicationDbContext.Themes.FirstOrDefault();
            return Task.FromResult(theme != null ? new GetThemeResponse(theme) : new GetThemeResponse(ReturnType.NotFound, string.Empty));
        }
    }
}
