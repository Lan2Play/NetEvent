using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class PutEventTicketType
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
                var result = await _EventManager.UpdateTicketAsync(newEventTicketType).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newEventTicketType.ToEventTicketTypeDto());
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long id, EventTicketTypeDto eventTicketTypeDto)
            {
                Guard.IsNotNull(eventTicketTypeDto, nameof(eventTicketTypeDto));

                Id = id;
                EventTicketType = eventTicketTypeDto;
            }

            public long Id { get; }

            public EventTicketTypeDto EventTicketType { get; }
        }

        public sealed class Response : ResponseBase<EventTicketTypeDto>
        {
            public Response(EventTicketTypeDto createdEventTicketType) : base(createdEventTicketType)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
