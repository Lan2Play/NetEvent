using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class SystemSettingValue
    {
        [Key]
        public string? Key { get; set; }

        public string? SerializedValue { get; set; }
    }
}
