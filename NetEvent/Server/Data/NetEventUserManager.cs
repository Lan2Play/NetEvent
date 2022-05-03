using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetEvent.Server.Models;

namespace NetEvent.Server.Data
{
    public class NetEventRoleManager : RoleManager<IdentityRole>
    {
        public NetEventRoleManager(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }

    public class NetEventUserManager : UserManager<ApplicationUser>
    {

        public NetEventUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        //public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        //{
        //    var result = await base.CreateAsync(user, password).ConfigureAwait(false);

        //    if (result.Succeeded)
        //    {
        //        await _TopicEventSender.SendAsync(nameof(Subscription.UserAdded), user).ConfigureAwait(false);
        //    }

        //    return result;
        //}

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await base.CreateAsync(user).ConfigureAwait(false);

            if (result.Succeeded)
            {
                try
                {
                    //await _TopicEventSender.SendAsync(nameof(Subscription.UserAdded), user).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                }
            }

            return result;
        }
    }
}
