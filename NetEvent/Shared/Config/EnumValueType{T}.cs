using System.Collections.Generic;

namespace NetEvent.Shared.Config
{
    public class EnumValueType<T> : ValueType<T>
        where T : class
    {
        public EnumValueType(T defaultValue, IReadOnlyCollection<T> enumItems) : base(defaultValue)
        {
            EnumItems = enumItems;
        }

        public override string DefaultValueSerialized => DefaultValue?.ToString() ?? string.Empty;

        public IReadOnlyCollection<T> EnumItems { get; }

        public override bool IsValid(T value)
        {
            throw new System.NotImplementedException();
        }
    }
}
