using System;
using System.Collections.Generic;
using System.Linq;

namespace NetEvent.Shared.Config
{
    public sealed class SystemSettings
    {
        private SystemSettings()
        {
            SettingsGroups = new List<ISettingsGroup>
            {
                new OrganizationData(),
                new StyleData(),
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
            public const string HideOrganizationNameInNavBar = "HideOrganizationNameInNavBar";
            public const string AboutUs = "AboutUs";
            public const string LegalNotice = "LegalNotice";
            public const string PrivacyPolicy = "PrivacyPolicy";

            public OrganizationData() : base(SystemSettingGroup.OrganizationData)
            {
                CreateSystemSetting(OrganizationName, new StringValueType("NetEvent"));
                CreateSystemSetting(HideOrganizationNameInNavBar, new BooleanValueType(false));
                CreateSystemSettingWithHint(DataCultureInfo, new EnumValueType<string>("en-US", new List<string> { "en-US", "de-DE", "fr-FR" }));
                CreateSystemSettingWithHint(Favicon, new ImageValueType());
                CreateSystemSetting(Logo, new ImageValueType());
                CreateSystemSettingWithHint(AboutUs, new StringValueType(string.Empty, true));
                CreateSystemSetting(LegalNotice, new StringValueType(string.Empty, true));
                CreateSystemSetting(PrivacyPolicy, new StringValueType(string.Empty, true));
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

        public class StyleData : SettingGroupBase
        {
            public const string PrimaryColor = "PrimaryColor";
            public const string PrimaryTextColor = "PrimaryTextColor";
            public const string SecondaryColor = "SecondaryColor";
            public const string SecondaryTextColor = "SecondaryTextColor";
            public const string Background = "Background";
            public const string AppbarBackground = "AppbarBackground";
            public const string AppbarText = "AppbarText";
            public const string CustomCss = "CustomCss";

            public StyleData() : base(SystemSettingGroup.StyleData)
            {
                CreateSystemSetting(PrimaryColor, new ColorValueType(string.Empty));
                CreateSystemSetting(PrimaryTextColor, new ColorValueType(string.Empty));
                CreateSystemSetting(SecondaryColor, new ColorValueType(string.Empty));
                CreateSystemSetting(SecondaryTextColor, new ColorValueType(string.Empty));
                CreateSystemSetting(Background, new ColorValueType(string.Empty));
                CreateSystemSetting(AppbarBackground, new ColorValueType(string.Empty));
                CreateSystemSetting(AppbarText, new ColorValueType(string.Empty));
                CreateSystemSetting(CustomCss, new StringValueType(string.Empty));
            }

            // All possible css variables are here https://mudblazor.com/customization/default-theme
            public static string[] GetCssVariables(string key)
            {
                return key switch
                {
                    PrimaryColor => new[] { "--mud-palette-primary" },
                    PrimaryTextColor => new[] { "--mud-palette-primary-text" },
                    SecondaryColor => new[] { "--mud-palette-secondary" },
                    SecondaryTextColor => new[] { "--mud-palette-secondary-text" },
                    Background => new[] { "--mud-palette-background", "--mud-palette-surface" },
                    AppbarBackground => new[] { "--mud-palette-appbar-background", "--mud-palette-drawer-background" },
                    AppbarText => new[] { "--mud-palette-appbar-text", "--mud-palette-drawer-background" },
                    _ => new[] { string.Empty },
                };
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
}
