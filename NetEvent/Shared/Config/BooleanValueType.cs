namespace NetEvent.Shared.Config
{
    public class BooleanValueType : ValueType<bool>
    {
        public BooleanValueType(bool defaultValue) : base(defaultValue)
        {
        }

        public override string DefaultValueSerialized => DefaultValue.ToString();

        public override bool IsValid(bool value) => true;

        public override bool IsValid(object value)
        {
            return bool.TryParse(value?.ToString(), out var _);
        }

        public static bool GetValue(string? serializedValue)
        {
            if (serializedValue != null && bool.TryParse(serializedValue, out var result))
            {
                return result;
            }

            return false;
        }
    }
}
