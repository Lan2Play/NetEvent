using System;

namespace NetEvent.Shared.Dto
{
    public class SystemImageDto
    {
        public SystemImageDto()
        {
        }

        public SystemImageDto(string? id, string? name, string? extension, DateTime? uploadTime)
        {
            Id = id;
            Name = name;
            Extension = extension;
            UploadTime = uploadTime;
        }

        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Extension { get; set; }

        public byte[]? Data { get; set; }

        public DateTime? UploadTime { get; set; }
    }
}
