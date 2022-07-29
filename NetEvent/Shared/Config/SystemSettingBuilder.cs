namespace NetEvent.Shared.Config
{
    public class SystemSettingBuilder
    {
        private SystemSettingBuilder(SystemSettingGroup settingGroup)
        {
            SettingGroup = settingGroup;
        }

        public static SystemSettingBuilder OrganizationBuilder { get; } = new SystemSettingBuilder(SystemSettingGroup.OrganizationData);

        public static SystemSettingBuilder AuthenticationBuilder { get; } = new SystemSettingBuilder(SystemSettingGroup.AuthenticationData);

        public SystemSettingGroup SettingGroup { get; }

        public SystemSetting CreateSystemSetting(string key, ValueType valueType) => new SystemSetting(SettingGroup, key, valueType, $"{nameof(SystemSetting)}.{SettingGroup}.{key}.Label", string.Empty);

        public SystemSetting CreateSystemSettingWithHint(string key, ValueType valueType) => new SystemSetting(SettingGroup, key, valueType, $"{nameof(SystemSetting)}.{SettingGroup}.{key}.Label", $"{nameof(SystemSetting)}.{SettingGroup}.{key}.Hint");
    }
}
