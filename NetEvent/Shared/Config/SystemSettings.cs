using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetEvent.Shared.Config;

namespace NetEvent.Shared.Config
{
    public interface ISettingsGroup
    {
        SystemSettingGroup SettingGroup { get; }

        IReadOnlyCollection<SystemSetting> Settings { get; }
    }

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

    public class SystemSettings
    {

        private SystemSettings()
        {
            SettingsGroups = new List<ISettingsGroup>
            {
                new OrganizationData(),
                new AuthenticationData()
            };
        }

        public static SystemSettings Instance { get; } = new SystemSettings();

        public List<ISettingsGroup> SettingsGroups { get; }

        public class OrganizationData : SettingGroupBase
        {
            public const string OrganizationName = "OrganizationName";
            public const string DataCultureInfo = "DataCultureInfo";
            public const string Favicon = "Favicon";
            public const string Logo = "Logo";
            public const string AboutUs = "AboutUs";
            public const string LegalNotice = "LegalNotice";
            public const string PrivacyPolicy = "PrivacyPolicy";

            public OrganizationData() : base(SystemSettingGroup.OrganizationData)
            {
                CreateSystemSettingWithHint(OrganizationName, new StringValueType("NetEvent"));
                CreateSystemSettingWithHint(DataCultureInfo, new EnumValueType<string>("en-US", new List<string> { "en-US", "de-DE", "fr-FR" }));
                CreateSystemSettingWithHint(Favicon, new ImageValueType());
                CreateSystemSettingWithHint(Logo, new ImageValueType());
                CreateSystemSettingWithHint(AboutUs, new StringValueType(string.Empty, true));
                CreateSystemSettingWithHint(LegalNotice, new StringValueType(string.Empty, true));
                CreateSystemSettingWithHint(PrivacyPolicy, new StringValueType(string.Empty, true));
            }
        }

        public class AuthenticationData : SettingGroupBase
        {
            public const string Standard = "Standard";
            public const string Steam = "Steam";

            public AuthenticationData() : base(SystemSettingGroup.AuthenticationData)
            {
                CreateSystemSettingWithHint(Standard, new BooleanValueType(true));
                CreateSystemSettingWithHint(Steam, new BooleanValueType(false));
            }
        }

        public static IEnumerable<string> GetSettingLabelKeys(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                var systemSetting = Instance.SettingsGroups.SelectMany(x => x.Settings).FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(systemSetting?.LabelKey))
                {
                    yield return systemSetting.LabelKey;
                }
            }
        }
    }

    //public class SystemSettings
    //{
    //    private SystemSettings()
    //    {
    //        Settings = new Dictionary<SystemSettingGroup, IReadOnlyCollection<SystemSetting>>
    //        {
    //            {
    //                SystemSettingGroup.OrganizationData,
    //                new Collection<SystemSetting>
    //                {
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(OrganizationName, new StringValueType("NetEvent")),
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(DataCultureInfo, new EnumValueType<string>("en-US", new List<string> { "en-US", "de-DE", "fr-FR" })),
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(Favicon, new ImageValueType()),
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(Logo, new ImageValueType()),
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(AboutUs, new StringValueType(string.Empty, true)),
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(LegalNotice, new StringValueType(string.Empty, true)),
    //                    SystemSettingBuilder.OrganizationBuilder.CreateSystemSettingWithHint(PrivacyPolicy, new StringValueType(string.Empty, true)),
    //                }
    //            },
    //            {
    //                SystemSettingGroup.AuthenticationData,
    //                new Collection<SystemSetting>
    //                {
    //                    SystemSettingBuilder.AuthenticationBuilder.CreateSystemSettingWithHint(Standard, new BooleanValueType(true)),
    //                    SystemSettingBuilder.AuthenticationBuilder.CreateSystemSettingWithHint(Steam, new BooleanValueType(false)),
    //                }
    //            },
    //        };
    //    }

    //    public static SystemSettings Instance { get; } = new SystemSettings();

    //    public IReadOnlyDictionary<SystemSettingGroup, IReadOnlyCollection<SystemSetting>> Settings { get; }

    //    #region OrganizationData

    //    public IReadOnlyCollection<SystemSetting> OrganizationData => Settings[SystemSettingGroup.OrganizationData];

    //    public const string OrganizationName = "OrganizationName";
    //    public const string DataCultureInfo = "DataCultureInfo";
    //    public const string Favicon = "Favicon";
    //    public const string Logo = "Logo";
    //    public const string AboutUs = "AboutUs";
    //    public const string LegalNotice = "LegalNotice";
    //    public const string PrivacyPolicy = "PrivacyPolicy";

    //    #endregion

    //    #region Authentication

    //    public IReadOnlyCollection<SystemSetting> AuthenticationData => Settings[SystemSettingGroup.AuthenticationData];

    //    public const string Standard = "Standard";
    //    public const string Steam = "Steam";

    //    #endregion

    //    #region HelperMethods

    //    public static IEnumerable<string> GetSettingLabelKeys(IEnumerable<string> keys)
    //    {
    //        foreach (var key in keys)
    //        {
    //            var systemSetting = Instance.OrganizationData.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
    //            if (!string.IsNullOrEmpty(systemSetting?.LabelKey))
    //            {
    //                yield return systemSetting.LabelKey;
    //            }
    //        }
    //    }

    //    #endregion
    //}
}
