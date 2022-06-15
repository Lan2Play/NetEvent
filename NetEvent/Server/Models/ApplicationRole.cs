using Microsoft.AspNetCore.Identity;

namespace NetEvent.Server.Models;

public class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
}
