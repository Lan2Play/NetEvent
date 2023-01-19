using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Events.Endpoints;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events
{
    public class EventsModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapModuleEndpoints(IEndpointRouteBuilder endpoints)
        {
            // BaseRoute: /api/events
            endpoints.MapGet("/upcoming", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetUpcomingEvent.Request())));
            endpoints.MapGet("/{eventId:long}", async ([FromRoute] long eventId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetEvent.Request(eventId))));
            endpoints.MapGet("/{slug}", async ([FromRoute] string slug, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetEvent.Request(slug))));
            endpoints.MapGet("/", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetEvents.Request())));

            return endpoints;
        }

        public override IEndpointRouteBuilder MapModuleWriteAuthEndpoints(IEndpointRouteBuilder endpoints)
        {
            // BaseRoute: /api/events
            endpoints.MapPost("/", async ([FromBody] EventDto eventDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostEvent.Request(eventDto))));
            endpoints.MapPut("/{eventId}", async ([FromRoute] long eventId, [FromBody] EventDto eventDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutEvent.Request(eventId, eventDto))));
            endpoints.MapDelete("/{eventId}", async ([FromRoute] long eventId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteEvent.Request(eventId))));

            var ticketTypeEndPoints = endpoints.MapGroup("/tickettype");
            ticketTypeEndPoints.MapPost("/{eventId}", async ([FromRoute] long eventId, [FromBody] EventTicketTypeDto eventTicketTypeDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostEventTicketType.Request(eventId, eventTicketTypeDto))));
            ticketTypeEndPoints.MapPut("/{eventTicketTypeId}", async ([FromRoute] long eventTicketTypeId, [FromBody] EventTicketTypeDto eventTicketTypeDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutEventTicketType.Request(eventTicketTypeId, eventTicketTypeDto))));
            ticketTypeEndPoints.MapDelete("/{eventTicketTypeId}", async ([FromRoute] long eventTicketTypeId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteEventTicketType.Request(eventTicketTypeId))));
            ticketTypeEndPoints.MapGet("/{eventTicketTypeId}", async ([FromRoute] long eventTicketTypeId, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetEventTicketType.Request(eventTicketTypeId))));

            return base.MapModuleWriteAuthEndpoints(endpoints);
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>(entity =>
            {
                entity.ToTable(name: "Events");
            });

            builder.Entity<EventTicketType>(entity =>
            {
                entity.ToTable(name: "EventTicketTypes");
            });
        }
    }
}
