using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Toolkit.Diagnostics;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class PostEvent
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
                var newEvent = request.Event.ToEvent();
                var result = await _EventManager.CreateAsync(newEvent).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newEvent.ToEventDto());
            }
        }

        public class Request : IRequest<Response>
        {
            public Request(EventDto eventDto)
            {
                Guard.IsNotNull(eventDto, nameof(eventDto));
                Guard.IsNull(eventDto.Id, nameof(eventDto));
                Event = eventDto;
            }

            public EventDto Event { get; }
        }

        public class Response : ResponseBase<EventDto>
        {
            public Response(EventDto createdEvent) : base(createdEvent)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
