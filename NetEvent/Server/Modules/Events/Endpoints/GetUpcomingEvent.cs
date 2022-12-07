using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class GetUpcomingEvent
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _DbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _DbContext = dbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var eventModel = _DbContext.Events.OrderBy(e => e.StartDate).FirstOrDefault(e => e.StartDate > DateTime.UtcNow);
                if (eventModel == null)
                {
                    return new Response(ReturnType.NotFound, "No upcoming Event!");
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

        public class Request : IRequest<Response>
        {
            public Request()
            {
            }
        }

        public class Response : ResponseBase<EventDto>
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
