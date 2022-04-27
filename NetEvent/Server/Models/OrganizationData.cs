using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class OrganizationData
    {
        public OrganizationData(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [Key]
        public string Key { get; }

        public string Value { get; }
    }
}
