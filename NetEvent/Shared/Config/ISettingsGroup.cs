using System.Collections.Generic;

namespace NetEvent.Shared.Config
{
    public interface ISettingsGroup
    {
        SystemSettingGroup SettingGroup { get; }

        IReadOnlyCollection<SystemSetting> Settings { get; }
    }
}
