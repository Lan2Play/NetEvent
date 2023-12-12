using System.Collections.Generic;

namespace NetEvent.Shared.Dto
{
    public class SystemInfoDto
    {
        public SystemInfoDto()
        {
        }

        public SystemInfoDto(IReadOnlyList<SystemInfoComponentEntryDto> components, IReadOnlyList<SystemInfoHealthEntryDto> health, IReadOnlyList<SystemInfoVersionEntryDto> versions)
        {
            Components = components;
            Health = health;
            Versions = versions;
        }

        public IReadOnlyList<SystemInfoComponentEntryDto> Components { get; set; } = default!;

        public IReadOnlyList<SystemInfoHealthEntryDto> Health { get; set; } = default!;

        public IReadOnlyList<SystemInfoVersionEntryDto> Versions { get; set; } = default!;
    }
}
