using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NetEvent.Server.Models
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Event
    {
        [Key]
        public long? Id { get; set; }

        public string? Name { get; set; }

        public string? Slug { get; set; }

        public PublishState State { get; set; }

        public VisibilityState Visibility { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public EventFormat EventFormat { get; set; }

        public long? VenueId { get; set; }

        [ForeignKey(nameof(VenueId))]
        public Venue? Venue { get; set; }

        [InverseProperty(nameof(EventTicketType.Event))]
        public List<EventTicketType>? TicketTypes { get; set; }
    }
}
