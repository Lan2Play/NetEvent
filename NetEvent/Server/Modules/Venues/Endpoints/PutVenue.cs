using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Venues.Endpoints
{
    public static class PutVenue
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventManager _EventManager;

            public Handler(IEventManager eventManager)
            {
                _EventManager = eventManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var newVenue = request.Venue.ToVenue();
                var result = await _EventManager.UpdateVenueAsync(newVenue).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newVenue.ToVenueDto());
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(long id, VenueDto venueDto)
            {
                Guard.IsNotNull(venueDto, nameof(venueDto));
                Guard.IsNotNull(venueDto!.Id, nameof(venueDto));

                Id = id;
                Venue = venueDto;
            }

            public long Id { get; }

            public VenueDto Venue { get; }
        }

        public class Response : ResponseBase<VenueDto>
        {
            public Response(VenueDto createdVenue) : base(createdVenue)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
