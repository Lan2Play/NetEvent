using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NetEvent.Shared.Policy
{
    public class RegexClaimsAuthorizationRequirement : AuthorizationHandler<RegexClaimsAuthorizationRequirement>, IAuthorizationRequirement
    {
        private readonly string _Claim;
        private readonly Regex _ClaimRegex;

        public RegexClaimsAuthorizationRequirement(string claimRegEx)
        {
            if (claimRegEx == null)
            {
                throw new ArgumentNullException(nameof(claimRegEx));
            }

            _Claim = claimRegEx;
            _ClaimRegex = new Regex(claimRegEx, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RegexClaimsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.Claims.Any((c) => _ClaimRegex.IsMatch(c.Type)))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail(new AuthorizationFailureReason(this, $"User needs claim \"{_Claim}\"!"));
                }
            }
            else
            {
                context.Fail(new AuthorizationFailureReason(this, $"User is required!"));
            }

            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return "RegexClaimsAuthorizationRequirement:Claim.Type=" + _Claim;
        }
    }
}
