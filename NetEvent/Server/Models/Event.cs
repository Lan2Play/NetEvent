using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class Event
    {
        [Key]
        public long? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
