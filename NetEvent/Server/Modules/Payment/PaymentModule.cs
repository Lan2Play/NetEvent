using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;
using NetEvent.Server.Modules;
using NetEvent.Server.Modules.Payment.Endpoints;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetVenue.Server.Modules.Payment
{
    public class PaymentModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapModuleEndpoints(IEndpointRouteBuilder endpoints)
        {
            // BaseRoute: /api/payment
            endpoints.MapPost("/buy", async ([FromBody] CartDto cartDto, ClaimsPrincipal user, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostCart.Request(cartDto, user))));
            //endpoints.MapGet("/{venueId:long}", async ([FromRoute] long venueId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetVenue.Request(venueId))));
            //endpoints.MapGet("/{slug}", async ([FromRoute] string slug, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetVenue.Request(slug))));
            //endpoints.MapGet("/", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetVenues.Request())));
            //endpoints.MapPost("/", async ([FromBody] VenueDto venueDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostVenue.Request(venueDto))));
            //endpoints.MapPut("/{venueId}", async ([FromRoute] long venueId, [FromBody] VenueDto venueDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutVenue.Request(venueId, venueDto))));
            //endpoints.MapDelete("/{venueId}", async ([FromRoute] long venueId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteVenue.Request(venueId))));

            return endpoints;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Purchase>(entity =>
            {
                entity.ToTable(name: "Purchases");
            });
            builder.Entity<TicketPurchase>(entity =>
            {
                entity.ToTable(name: "TicketPurchases");
            });
        }
    }
}
