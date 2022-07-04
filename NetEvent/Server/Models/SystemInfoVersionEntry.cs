using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class SystemInfoVersionEntry
    {
        [Key]
        public string? Component { get; set; }

        public string? Version { get; set; }
    }
}
