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
    public static class GetEvent
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
                Models.Event? eventModel;
                if (request.Slug != null)
                {
                    eventModel = await _DbContext.Events.Where(e => e.Slug == request.Slug).Include(e => e.TicketTypes).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    eventModel = await _DbContext.Events.Where(x => x.Id == request.Id).Include(e => e.TicketTypes).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
                }

                if (eventModel == null)
                {
                    return new Response(ReturnType.NotFound, "Event for Id not found!");
                }

                var convertedEvent = eventModel.ToEventDto();

                if (eventModel.VenueId.HasValue)
                {
                    var venue = await _DbContext.Venues.FindAsync(new object[] { eventModel.VenueId }, cancellationToken);
                    convertedEvent.Venue = venue?.ToVenueDto();
                }

                return new Response(convertedEvent);
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

        public sealed class Response : ResponseBase<EventDto>
        {
            public Response(EventDto? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
