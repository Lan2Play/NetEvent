using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetEvent.Shared.Config
{

    public abstract class SettingGroupBase : ISettingsGroup
    {
        private readonly Collection<SystemSetting> _Settings = new();

        protected SettingGroupBase(SystemSettingGroup settingGroup)
        {
            SettingGroup = settingGroup;
        }

        public SystemSettingGroup SettingGroup { get; }

        public IReadOnlyCollection<SystemSetting> Settings => _Settings;

        protected void CreateSystemSetting(string key, ValueType valueType) => _Settings.Add(new SystemSetting(SettingGroup, key, valueType, $"{nameof(SystemSetting)}.{SettingGroup}.{key}.Label", string.Empty));

        protected void CreateSystemSettingWithHint(string key, ValueType valueType) => _Settings.Add(new SystemSetting(SettingGroup, key, valueType, $"{nameof(SystemSetting)}.{SettingGroup}.{key}.Label", $"{nameof(SystemSetting)}.{SettingGroup}.{key}.Hint"));
    }
}
