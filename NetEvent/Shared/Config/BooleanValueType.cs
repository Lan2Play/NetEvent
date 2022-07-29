namespace NetEvent.Shared.Config
{
    public class BooleanValueType : ValueType<bool>
    {
        public BooleanValueType(bool defaultValue) : base(defaultValue)
        {
        }

        public override string DefaultValueSerialized => DefaultValue.ToString();

        public override bool IsValid(bool value) => true;

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
