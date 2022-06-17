using Microsoft.AspNetCore.Identity;

namespace NetEvent.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public byte[]? ProfilePicture { get; set; }
}
