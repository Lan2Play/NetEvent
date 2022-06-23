namespace NetEvent.Shared.Dto
{
    public class SystemInfoDto
    {
        public SystemInfoDto()
        {

        }

        public SystemInfoDto(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; } = default!;

        public string Value { get; set; } = default!;
    }
}
