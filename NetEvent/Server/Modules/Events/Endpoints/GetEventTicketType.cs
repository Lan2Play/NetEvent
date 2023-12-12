using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class GetEventTicketType
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _DbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _DbContext = dbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Models.EventTicketType? eventTicketTypeModel;
                if (request.Slug != null)
                {
                    eventTicketTypeModel = await _DbContext.Tickets.Where(e => e.Slug != null && e.Slug.Equals(request.Slug, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    eventTicketTypeModel = await _DbContext.Tickets.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
                }

                if (eventTicketTypeModel == null)
                {
                    return new Response(ReturnType.NotFound, "Event for Id not found!");
                }

                var convertedEventTicketType = eventTicketTypeModel.ToEventTicketTypeDto();

                return new Response(convertedEventTicketType);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long id)
            {
                Id = id;
            }

            public Request(string slug)
            {
                Slug = slug;
            }

            public long Id { get; }

            public string? Slug { get; }
        }

        public sealed class Response : ResponseBase<EventTicketTypeDto>
        {
            public Response(EventTicketTypeDto? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
