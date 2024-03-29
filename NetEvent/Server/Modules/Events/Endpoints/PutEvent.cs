﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class PutEvent
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
                var newEvent = request.Event.ToEvent();
                var result = await _EventManager.UpdateAsync(newEvent).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newEvent.ToEventDto());
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long id, EventDto eventDto)
            {
                Guard.IsNotNull(eventDto, nameof(eventDto));
                Guard.IsNotNull(eventDto!.Id, nameof(eventDto));

                Id = id;
                Event = eventDto;
            }

            public long Id { get; }

            public EventDto Event { get; }
        }

        public sealed class Response : ResponseBase<EventDto>
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
