namespace NetEvent.Shared.Config
{
    public class ImageValueType : ValueType<string>
    {
        internal ImageValueType() : base(string.Empty)
        {
        }

        public override string DefaultValueSerialized => DefaultValue;

        public override bool IsValid(string value) => true;
    }
}
