using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace NetEvent.Server.Data
{
    public class NetEventRoleManager : RoleManager<IdentityRole>
    {
        public NetEventRoleManager(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
