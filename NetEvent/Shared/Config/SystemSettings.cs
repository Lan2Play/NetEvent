using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetEvent.Shared.Config
{
    public class SystemSettings
    {
        private SystemSettings()
        {
            OrganizationData = new Collection<SystemSetting>
            {
                SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(OrganizationName, new StringValueType("NetEvent")),
                SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(DataCultureInfo, new EnumValueType<string>("en-US", new List<string> { "en-US", "de-DE", "fr-FR" })),
                SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(Favicon, new ImageValueType()),
                SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(Logo, new ImageValueType()),
            };
        }

        public static SystemSettings Instance { get; } = new SystemSettings();

        public IReadOnlyCollection<SystemSetting> OrganizationData { get; }

        public const string OrganizationName = "OrganizationName";
        public const string DataCultureInfo = "DataCultureInfo";
        public const string Favicon = "Favicon";
        public const string Logo = "Logo";
    }
}
