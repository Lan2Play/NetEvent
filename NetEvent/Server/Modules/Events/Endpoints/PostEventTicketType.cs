using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class PostEventTicketType
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
                var newEventTicketType = request.EventTicketType.ToEventTicketType();
                newEventTicketType.EventId = request.EventId;

                var result = await _EventManager.CreateTicketAsync(newEventTicketType).ConfigureAwait(false);
                if (!result.Succeeded || newEventTicketType.Id == null)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newEventTicketType.Id!.Value);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long eventId, EventTicketTypeDto eventTicketTypeDto)
            {
                EventId = eventId;
                EventTicketType = eventTicketTypeDto;
            }

            public long EventId { get; }

            public EventTicketTypeDto EventTicketType { get; }
        }

        public sealed class Response : ResponseBase<long>
        {
            public Response(long createdEventTicketTypeId) : base(createdEventTicketTypeId)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
