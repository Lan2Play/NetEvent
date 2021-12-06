using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetEvent.Server.GraphQl;
using NetEvent.Server.Models;

namespace NetEvent.Server.Data
{
    public class NetEventUserManager : UserManager<ApplicationUser>
    {
        private readonly ITopicEventSender _TopicEventSender;

        public NetEventUserManager(ITopicEventSender topicEventSender, IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _TopicEventSender = topicEventSender;
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
                await _TopicEventSender.SendAsync(nameof(Subscription.UserAdded), user).ConfigureAwait(false);
            }

            return result;
        }
    }
}
