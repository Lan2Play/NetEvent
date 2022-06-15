using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Themes.Endpoints.PutTheme
{
    public class PutThemeHandler : IRequestHandler<PutThemeRequest, PutThemeResponse>
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public PutThemeHandler(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        public async Task<PutThemeResponse> Handle(PutThemeRequest request, CancellationToken cancellationToken)
        {
            var oldTheme = request.Theme.Id != Guid.Empty ? await _ApplicationDbContext.Themes.FindAsync(new object[] { request.Theme.Id }, cancellationToken) : _ApplicationDbContext.Themes.FirstOrDefault();
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

            await _ApplicationDbContext.SaveChangesAsync(cancellationToken);

            return new PutThemeResponse();
        }
    }
}
