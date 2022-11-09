namespace NetEvent.Shared.Config
{
    public class ColorValueType : RegexStringValueType
    {
        internal ColorValueType(string defaultColor) : base(defaultColor, "^$|^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$")
        {
        }
    }
}
