namespace NetEvent.Shared.Config
{
    public abstract class ValueType
    {
        protected ValueType()
        {
        }

        public abstract string DefaultValueSerialized { get; }
    }

    public abstract class ValueType<T> : ValueType
    {
        protected ValueType(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public T DefaultValue { get; set; }

        public abstract bool IsValid(T value);
    }
}
