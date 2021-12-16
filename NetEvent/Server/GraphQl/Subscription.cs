using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Subscription
    {
        [Subscribe]
        public ApplicationUser UserAdded([EventMessage] ApplicationUser user) => user;

        [Subscribe]
        public IdentityUserRole<string> UserRoleAdded([EventMessage] IdentityUserRole<string> userRole) => userRole;
    }
}
