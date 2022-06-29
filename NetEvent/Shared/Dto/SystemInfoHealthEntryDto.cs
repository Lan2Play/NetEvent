using System.Collections.Generic;

namespace NetEvent.Shared.Dto
{
    public class SystemInfoHealthEntryDto
    {
        public SystemInfoHealthEntryDto()
        {

        }

        public SystemInfoHealthEntryDto(string component, string value, bool healthy)
        {
            Component = component;
            Value = value;
            Healthy = healthy;
        }

        public string Component { get; set; } = default!;

        public string Value { get; set; } = default!;

        public bool Healthy { get; set; } = default!;

    }


}
