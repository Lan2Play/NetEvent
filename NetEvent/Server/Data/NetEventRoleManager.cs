using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;

namespace NetEvent.Server.Data
{
    public class NetEventRoleManager : RoleManager<ApplicationRole>
    {
        public NetEventRoleManager(IRoleStore<ApplicationRole> store, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            var result = await base.CreateAsync(role);
            await UpdateIsDefaultAsync(role, result);
            return result;
        }

        protected override async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
        {
            var result = await base.UpdateRoleAsync(role);
            await UpdateIsDefaultAsync(role, result);
            return result;
        }

        public override Task<IdentityResult> DeleteAsync(ApplicationRole role)
        {
            if (role.IsDefault)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "NotAllowed", Description = "Default Role can not be deleted!" }));
            }

            return base.DeleteAsync(role);
        }

        private async Task UpdateIsDefaultAsync(ApplicationRole role, IdentityResult result)
        {
            if (result.Succeeded && role.IsDefault)
            {
                var rolesToUnsetDefault = Roles.Where(x => x.Id != role.Id && x.IsDefault).ToList();
                foreach (var roleToUnset in rolesToUnsetDefault)
                {
                    roleToUnset.IsDefault = false;
                    await UpdateAsync(roleToUnset);
                }
            }
        }
    }
}
