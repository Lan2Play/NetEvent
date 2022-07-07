using System;
using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models
{
    public class SystemImage
    {
        [Key]
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public byte[] Data { get; set; }

        public DateTime UploadTime { get; set; }
    }
}
