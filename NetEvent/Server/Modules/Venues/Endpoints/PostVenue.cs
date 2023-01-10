using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Venues.Endpoints
{
    public static class PostVenue
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventManager _EventManager;

            public Handler(IEventManager eventManager)
            {
                _EventManager = eventManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var newVenue = request.Venue.ToVenue();
                var result = await _EventManager.CreateVenueAsync(newVenue).ConfigureAwait(false);
                if (!result.Succeeded || newVenue.Id == null)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newVenue.Id!.Value);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(VenueDto venueDto)
            {
                Venue = venueDto;
            }

            public VenueDto Venue { get; }
        }

        public sealed class Response : ResponseBase<long>
        {
            public Response(long createdVenueId) : base(createdVenueId)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
