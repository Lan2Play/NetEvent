using System.Collections.Generic;

namespace NetEvent.Server.Models
{
    public class SystemInfo
    {
        public IList<SystemInfoComponentEntry>? Components { get; set; }

        public IList<SystemInfoHealthEntry>? Health { get; set; }

        public IList<SystemInfoVersionEntry>? Versions { get; set; }
    }
}
