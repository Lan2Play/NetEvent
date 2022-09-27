using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;
using NetEvent.Server.Modules;
using NetEvent.Server.Modules.Venues.Endpoints;
using NetEvent.Shared.Dto.Event;

namespace NetVenue.Server.Modules.Venues
{
    public class VenuesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/venues/{venueId:long}", async ([FromRoute] long venueId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetVenue.Request(venueId))));
            endpoints.MapGet("/api/venues/{slug}", async ([FromRoute] string slug, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetVenue.Request(slug))));
            endpoints.MapGet("/api/venues", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetVenues.Request())));
            endpoints.MapPost("/api/venue", async ([FromBody] VenueDto venueDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostVenue.Request(venueDto))));
            endpoints.MapPut("/api/venue/{venueId}", async ([FromRoute] long venueId, [FromBody] VenueDto venueDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutVenue.Request(venueId, venueDto))));
            endpoints.MapDelete("/api/venue/{venueId}", async ([FromRoute] long venueId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteVenue.Request(venueId))));

            return endpoints;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Venue>(entity =>
            {
                entity.ToTable(name: "Venues");
            });
        }
    }
}
