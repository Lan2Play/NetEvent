using System.Text.RegularExpressions;

namespace NetEvent.Shared.Config
{

    public class RegexStringValueType : ValueType<string>
    {
        private Regex _ValidationRegex;

        // TODO Add StringSyntax Attribute with .Net7
        internal RegexStringValueType(string defaultValue, /*[StringSyntax(...)]*/ string validationRegEx) : base(defaultValue)
        {
            _ValidationRegex = new Regex(validationRegEx, RegexOptions.Compiled);
        }

        public override string DefaultValueSerialized => DefaultValue;

        public override bool IsValid(string value)
        {
            return _ValidationRegex.IsMatch(value);
        }
    }
}
