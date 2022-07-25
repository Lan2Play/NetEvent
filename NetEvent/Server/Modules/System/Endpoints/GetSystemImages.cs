using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints
{
    public static class GetSystemImages
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allImages = _ApplicationDbContext.SystemImages.AsQueryable().Select(x => DtoMapper.Mapper.SystemImageToSystemImageDto(x)).ToList();
                return Task.FromResult(new Response(allImages));
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request()
            {
            }
        }

        public sealed class Response : ResponseBase<IEnumerable<SystemImageDto>>
        {
            public Response(IEnumerable<SystemImageDto> result) : base(result)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }

}
