﻿namespace NetEvent.Shared.Config
{
    public abstract class ValueType<T> : ValueType
    {
        protected ValueType(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public T DefaultValue { get; set; }

        public abstract bool IsValid(T value);

        public override bool IsValid(object value)
        {
            if (value is T t)
            {
                return IsValid(t);
            }

            return false;
        }
    }
}
