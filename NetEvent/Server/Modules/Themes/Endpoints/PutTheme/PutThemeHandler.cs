using MediatR;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class PutThemeHandler : IRequestHandler<PutThemeRequest, PutThemeResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly ILogger<PutThemeHandler> _Logger;

        public PutThemeHandler(ApplicationDbContext applicationDbContext, ILogger<PutThemeHandler> logger)
        {
            _ApplicationDbContext = applicationDbContext;
            _Logger = logger;
        }

        public async Task<PutThemeResponse> Handle(PutThemeRequest request, CancellationToken cancellationToken)
        {
            var oldTheme = request.Theme.Id != default ? await _ApplicationDbContext.Themes.FindAsync(request.Theme.Id) : null;
            if (oldTheme != null)
            {
                oldTheme.ThemeData = request.Theme.ThemeData;
                _ApplicationDbContext.Themes.Update(oldTheme);
            }
            else
            {
                request.Theme.Id = Guid.NewGuid();
                await _ApplicationDbContext.Themes.AddAsync(request.Theme, cancellationToken);
            }

            await _ApplicationDbContext.SaveChangesAsync();

            return new PutThemeResponse();
        }
    }
}
