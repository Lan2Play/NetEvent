using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
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

            if (theme == null)
            {
                var errorMessage = "No theme set.";
                _Logger.LogError(errorMessage);
                return Task.FromResult(new GetThemeResponse(ReturnType.NotFound, errorMessage));
            }

            return Task.FromResult(new GetThemeResponse(theme));
        }
    }
}
