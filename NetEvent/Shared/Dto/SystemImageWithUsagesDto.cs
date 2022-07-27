using System.Collections.Generic;
using System.Linq;

namespace NetEvent.Shared.Dto
{
    public class SystemImageWithUsagesDto
    {
        public SystemImageWithUsagesDto(SystemImageDto image, IReadOnlyCollection<string> settingUsages)
        {
            Image = image;
            SettingUsages = settingUsages;
        }

        public SystemImageDto Image { get; }

        public IReadOnlyCollection<string> SettingUsages { get; }

        public bool IsUsed => SettingUsages.Any();
    }
}
