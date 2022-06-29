using System.Collections.Generic;

namespace NetEvent.Shared.Dto
{
    public class SystemInfoDto
    {
        public SystemInfoDto()
        {

        }

        public SystemInfoDto(List<SystemInfoComponentEntryDto> components, List<SystemInfoHealthEntryDto> health, List<SystemInfoVersionEntryDto> versions)
        {
            Components = components;
            Health = health;
            Versions = versions;
        }

        public List<SystemInfoComponentEntryDto> Components { get; set; } = default!;

        public List<SystemInfoHealthEntryDto> Health { get; set; } = default!;

        public List<SystemInfoVersionEntryDto> Versions { get; set; } = default!;

    }


}
