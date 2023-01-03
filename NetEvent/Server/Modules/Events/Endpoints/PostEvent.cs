using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class PostEvent
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
                var result = await _EventManager.CreateAsync(newEvent).ConfigureAwait(false);
                if (!result.Succeeded || newEvent.Id == null)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response(newEvent.Id!.Value);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(EventDto eventDto)
            {
                Event = eventDto;
            }

            public EventDto Event { get; }
        }

        public sealed class Response : ResponseBase<long>
        {
            public Response(long createdEventId) : base(createdEventId)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
