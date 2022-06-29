using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{

    public class SystemInfoComponentEntry
    {
        [Key]
        public string? Component { get; set; }

        public string? Version { get; set; }
    }

}
