namespace NetEvent.Shared.Dto
{
    public class OrganizationDataDto
    {
        public OrganizationDataDto(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }
    }
}
