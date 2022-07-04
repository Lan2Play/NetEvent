namespace NetEvent.Shared.Dto
{
    public class SystemSettingValueDto
    {
        public SystemSettingValueDto()
        {
        }

        public SystemSettingValueDto(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; } = default!;

        public string Value { get; set; } = default!;
    }
}
