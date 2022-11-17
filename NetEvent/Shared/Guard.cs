using System;

namespace NetEvent.Shared
{
    public static class Guard
    {
        public static void IsNotNull(object? o, string name)
        {
            if (o == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void IsNotNullOrEmpty(string? text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
