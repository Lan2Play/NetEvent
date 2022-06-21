namespace NetEvent.Shared.Config
{
    public class SystemSetting
    {
        internal SystemSetting(string settingType, string key, ValueType valueType)
        {
            SettingType = settingType;
            Key = key;
            ValueType = valueType;
        }

        public string SettingType { get; }

        public string Key { get; }

        public string TextKey => "TODO Label: " + Key;

        public ValueType ValueType { get; }
    }
}
