namespace NetEvent.Shared.Config
{
    public abstract class ValueType
    {
        protected ValueType()
        {
        }

        public abstract string DefaultValueSerialized { get; }

        public abstract bool IsValid(object value);
    }
}
