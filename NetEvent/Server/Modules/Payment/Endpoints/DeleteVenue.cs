using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;

namespace NetEvent.Server.Modules.Payment.Endpoints
{
    public static class DeleteVenue
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
                var result = await _EventManager.DeleteVenueAsync(request.VenueId).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long venueId)
            {
                VenueId = venueId;
            }

            public long VenueId { get; }
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
