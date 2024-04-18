using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetEvent.Shared.Dto.Event
{
    public class EventDto
    {
        public long? Id { get; set; } = -1;

        public string? Name { get; set; }

        public string? Slug { get; set; }

        public PublishStateDto State { get; set; }

        public VisibilityStateDto Visibility { get; set; }

        public VenueDto? Venue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public EventFormatDto EventFormat { get; set; }

        public ICollection<EventTicketTypeDto>? TicketTypes { get; set; } = new Collection<EventTicketTypeDto>();
    }
}
