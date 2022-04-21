using Microsoft.AspNetCore.Identity;

namespace NetEvent.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

    public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
}
