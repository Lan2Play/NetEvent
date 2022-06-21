using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetEvent.Shared.Config
{
    public class SystemSettings
    {
        public SystemSettings()
        {
            OrganizationData = new Collection<SystemSetting>
            {
                SystemSettingBuilder.OrganizationBuilder.CreateSystemSetting("OrganizationName", new StringValueType("NetEvent")),
                SystemSettingBuilder.OrganizationBuilder.CreateSystemSetting("DataCultureInfo", new StringValueType("en-US")),
            };
        }

        public IReadOnlyCollection<SystemSetting> OrganizationData { get; }
    }
}
