using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Payment.Endpoints
{
    public static class GetVenue
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _DbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _DbContext = dbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Models.Venue? venueModel;
                if (request.Slug != null)
                {
                    venueModel = _DbContext.Venues.Where(e => e.Slug != null && e.Slug.Equals(request.Slug, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
                else
                {
                    venueModel = await _DbContext.Venues.FindAsync(new object[] { request.Id }, cancellationToken);
                }

                if (venueModel == null)
                {
                    return new Response(ReturnType.NotFound, "Venue for Id not found!");
                }

                var convertedVenue = venueModel.ToVenueDto();

                return new Response(convertedVenue);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long id)
            {
                Id = id;
            }

            public Request(string slug)
            {
                Slug = slug;
            }

            public long Id { get; }

            public string? Slug { get; }
        }

        public sealed class Response : ResponseBase<VenueDto>
        {
            public Response(VenueDto? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
