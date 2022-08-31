using System.Collections.Generic;
using System.Linq;

namespace NetEvent.Shared.Dto
{
    public class SystemImageWithUsagesDto
    {
        public SystemImageWithUsagesDto(SystemImageDto image, IReadOnlyCollection<string> settingUsages, IReadOnlyCollection<string> eventUsages)
        {
            Image = image;
            SettingUsages = settingUsages;
            EventUsages = eventUsages;
        }

        public SystemImageDto Image { get; }

        public IReadOnlyCollection<string> SettingUsages { get; }

        public IReadOnlyCollection<string> EventUsages { get; }

        public bool IsUsed => SettingUsages.Any() || EventUsages.Any();
    }
}
