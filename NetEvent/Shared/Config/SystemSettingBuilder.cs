namespace NetEvent.Shared.Config
{
    public class SystemSettingBuilder
    {
        private SystemSettingBuilder(SystemSettingGroup settingGroup)
        {
            SettingGroup = settingGroup;
        }

        public static SystemSettingBuilder OrganizationBuilder { get; } = new SystemSettingBuilder(SystemSettingGroup.OrganizationData);

        public SystemSettingGroup SettingGroup { get; }

        public SystemSetting CreateSystemSetting(string key, ValueType valueType)
        {
            return new SystemSetting(SettingGroup.ToString(), key, valueType);
        }
    }
}
