using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Venues.Endpoints
{
    public static class GetVenues
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _DbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _DbContext = dbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allVenues = await _DbContext.Venues.ToListAsync(cancellationToken);
                var convertedVenues = allVenues.Select(DtoMapper.ToVenueDto).ToList();
                return new Response(convertedVenues);
            }
        }

        public class Request : IRequest<Response>
        {
        }

        public class Response : ResponseBase<IEnumerable<VenueDto>>
        {
            public Response(IEnumerable<VenueDto>? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
