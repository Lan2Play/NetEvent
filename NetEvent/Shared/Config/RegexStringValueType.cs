using System.Text.RegularExpressions;

namespace NetEvent.Shared.Config
{
    public class RegexStringValueType : ValueType<string>
    {
        private readonly Regex _ValidationRegex;

        // TODO Add StringSyntax Attribute with .Net7
        internal RegexStringValueType(string defaultValue, /*[StringSyntax(...)]*/ string validationRegEx) : this(defaultValue, validationRegEx, false)
        {
        }

        internal RegexStringValueType(string defaultValue, /*[StringSyntax(...)]*/ string validationRegEx, bool isRichtTextValue) : base(defaultValue)
        {
            _ValidationRegex = new Regex(validationRegEx, RegexOptions.Compiled);
            IsRichTextValue = isRichtTextValue;
        }

        public bool IsRichTextValue { get; }

        public override string DefaultValueSerialized => DefaultValue;

        public override bool IsValid(string value)
        {
            return _ValidationRegex.IsMatch(value);
        }
    }
}
