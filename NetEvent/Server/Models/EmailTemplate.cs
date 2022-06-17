using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class EmailTemplate
    {
        [Key]
        public string? TemplateId { get; set; }

        public string SubjectTemplate { get; set; } = default!;

        public string ContentTemplate { get; set; } = default!;
    }
}
