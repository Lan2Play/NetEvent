using System.Collections.Generic;

namespace NetEvent.Shared.Dto
{
    public class SystemInfoVersionEntryDto
    {
        public SystemInfoVersionEntryDto()
        {
        }

        public SystemInfoVersionEntryDto(string component, string version)
        {
            Component = component;
            Version = version;
        }

        public string Component { get; set; } = default!;

        public string Version { get; set; } = default!;
    }
}
