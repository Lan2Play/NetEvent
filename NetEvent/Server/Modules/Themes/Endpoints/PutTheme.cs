using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes.Endpoints
{
    public static class PutTheme
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
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

                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(ThemeDto theme)
            {
                Theme = theme;
            }

            public ThemeDto Theme { get; }
        }

        public sealed class Response : ResponseBase
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
