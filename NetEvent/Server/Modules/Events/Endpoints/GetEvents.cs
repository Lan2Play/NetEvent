﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Events.Endpoints
{
    public static class GetEvents
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
                var allEvents = await _DbContext.Events.ToListAsync(cancellationToken);
                var allLocations = await _DbContext.Locations.ToListAsync(cancellationToken);

                var convertedLocations = allLocations.Select(DtoMapper.ToLocationDto).ToList();

                var result = new List<EventDto>();
                foreach (var eventModel in allEvents)
                {
                    var convertedEvent = eventModel.ToEventDto();
                    if (eventModel.LocationId.HasValue)
                    {
                        var convertedLocation = convertedLocations.First(x => x.Id.Equals(eventModel.LocationId));
                        convertedEvent.Location = convertedLocation;
                    }

                    result.Add(convertedEvent);
                }

                return new Response(result);
            }
        }

        public class Request : IRequest<Response>
        {
        }

        public class Response : ResponseBase<IEnumerable<EventDto>>
        {
            public Response(IEnumerable<EventDto>? value) : base(value)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}