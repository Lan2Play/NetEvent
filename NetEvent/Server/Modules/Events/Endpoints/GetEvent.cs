using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class GetEvent
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
                Models.Event? eventModel;
                if (request.Slug != null)
                {
                    eventModel = _DbContext.Events.Where(e => e.Slug != null && e.Slug.Equals(request.Slug)).FirstOrDefault();
                }
                else
                {
                    eventModel = await _DbContext.Events.FindAsync(new object[] { request.Id }, cancellationToken);
                }

                if (eventModel == null)
                {
                    return new Response(ReturnType.NotFound, "Event for Id not found!");
                }

                var convertedEvent = eventModel.ToEventDto();

                if (eventModel.LocationId.HasValue)
                {
                    var location = await _DbContext.Locations.FindAsync(new object[] { eventModel.LocationId }, cancellationToken);
                    convertedEvent.Location = location?.ToLocationDto();
                }

                return new Response(convertedEvent);
            }
        }

        public class Request : IRequest<Response>
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
