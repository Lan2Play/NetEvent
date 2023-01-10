using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data.Events;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class DeleteEvent
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
                var result = await _EventManager.DeleteAsync(request.EventId).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return new Response(ReturnType.Error, string.Join(Environment.NewLine, result.Errors));
                }

                return new Response();
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long eventId)
            {
                EventId = eventId;
            }

            public long EventId { get; }
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
