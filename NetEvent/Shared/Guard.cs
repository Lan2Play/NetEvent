using System;
using System.Xml.Linq;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Shared
{
    public class Guard
    {
        public static void IsNotNull(object o, string name)
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
