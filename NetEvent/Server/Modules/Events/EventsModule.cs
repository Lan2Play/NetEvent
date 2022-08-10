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
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/events", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetEvents.Request())));
            endpoints.MapPost("/api/events", async ([FromBody] EventDto eventDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostEvent.Request(eventDto))));
            endpoints.MapPut("/api/events/{eventId}", async ([FromRoute] long eventId, [FromBody] EventDto eventDto, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutEvent.Request(eventId, eventDto))));
            endpoints.MapDelete("/api/events/{eventId}", async ([FromRoute] long eventId, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteEvent.Request(eventId))));

            return endpoints;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>(entity =>
            {
                entity.ToTable(name: "Events");
            });

            builder.Entity<Location>(entity =>
            {
                entity.ToTable(name: "Locations");
            });

            builder.Entity<EventLocation>(entity =>
            {
                entity.ToTable(name: "EventLocations");
            });
        }
    }
}
