using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
       
        public string LastName { get; set; }

        public byte[]? ProfilePicture { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        [JsonIgnore]
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
    }
}