using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class SystemInfo
    {
        public List<SystemInfoComponentEntry>? Components { get; set; }

        public List<SystemInfoHealthEntry>? Health { get; set; }

        public List<SystemInfoVersionEntry>? Versions { get; set; }
    }

}
