using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class Venue
    {
        [Key]
        public long? Id { get; set; }

        public string? Name { get; set; }

        public string? Slug { get; set; }

        public string? Street { get; set; }

        public string? Number { get; set; }

        public string? ZipCode { get; set; }

        public string? City { get; set; }
    }
}
