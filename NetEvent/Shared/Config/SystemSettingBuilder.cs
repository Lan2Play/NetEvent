namespace NetEvent.Shared.Config
{
    public class SystemSettingBuilder
    {
        public SystemSettingBuilder(SystemSettingGroup settingGroup)
        {
            SettingGroup = settingGroup;
        }

        public static SystemSettingBuilder OrganizationBuilder { get; } = new SystemSettingBuilder(SystemSettingGroup.OrganizationData);

        public static SystemSettingBuilder AuthenticationBuilder { get; } = new SystemSettingBuilder(SystemSettingGroup.AuthenticationData);

        public SystemSettingGroup SettingGroup { get; }

      
    }
}
