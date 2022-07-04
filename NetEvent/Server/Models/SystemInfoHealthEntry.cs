using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class SystemInfoHealthEntry
    {
        [Key]
        public string? Component { get; set; }

        public string? Value { get; set; }

        public bool? Healthy { get; set; }
    }
}
