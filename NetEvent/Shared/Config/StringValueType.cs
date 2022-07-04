namespace NetEvent.Shared.Config
{
    public class StringValueType : RegexStringValueType
    {
        internal StringValueType(string defaultValue) : base(defaultValue, ".*")
        {
        }

        internal StringValueType(string defaultValue, int maxLength) : base(defaultValue, $".*{{0:{maxLength}}}")
        {
        }

        internal StringValueType(string defaultValue, int minLength, int maxLength) : base(defaultValue, $".*{{{minLength}:{maxLength}}}")
        {
        }
    }
}
