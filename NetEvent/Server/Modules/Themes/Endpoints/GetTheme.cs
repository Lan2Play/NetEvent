using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Data;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes.Endpoints
{
    public static class GetTheme
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger<Handler> _Logger;
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext, ILogger<Handler> logger)
            {
                _ApplicationDbContext = applicationDbContext;
                _Logger = logger;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var theme = _ApplicationDbContext.Themes.OrderBy(a => a.Id).FirstOrDefault();

                if (theme == null)
                {
                    var errorMessage = "No theme set.";
                    _Logger.LogError(errorMessage);
                    return Task.FromResult(new Response(ReturnType.Error, errorMessage));
                }

                return Task.FromResult(new Response(theme));
            }
        }

        public sealed class Response : ResponseBase<ThemeDto>
        {
            public Response(ThemeDto? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }

        public sealed class Request : IRequest<Response>
        {
        }
    }
}
