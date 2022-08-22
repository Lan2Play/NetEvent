using System;
using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class Event
    {
        [Key]
        public long? Id { get; set; }

        public string? Name { get; set; }

        public PublishState State { get; set; }

        public VisibilityState Visibility { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long? LocationId { get; set; }
    }
}
