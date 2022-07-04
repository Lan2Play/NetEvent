namespace NetEvent.Shared.Config
{
    public class SystemSetting
    {
        internal SystemSetting(SystemSettingGroup settingType, string key, ValueType valueType, string labelKey, string descriptionKey)
        {
            SettingType = settingType;
            Key = key;
            ValueType = valueType;
            LabelKey = labelKey;
            DescriptionKey = descriptionKey;
        }

        public SystemSettingGroup SettingType { get; }

        public string Key { get; }

        public ValueType ValueType { get; }

        public string LabelKey { get; }

        public string DescriptionKey { get; }
    }
}
