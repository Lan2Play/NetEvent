using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NetEvent.Shared.Policy
{

    public class RegexClaimsAuthorizationRequirement : AuthorizationHandler<RegexClaimsAuthorizationRequirement>, IAuthorizationRequirement
    {
        public string ClaimRegEx { get; }

        public RegexClaimsAuthorizationRequirement(string claimRegEx)
        {
            if (claimRegEx == null)
            {
                throw new ArgumentNullException(nameof(claimRegEx));
            }

            ClaimRegEx = claimRegEx;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RegexClaimsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var claimRegex = new Regex(requirement.ClaimRegEx, RegexOptions.IgnoreCase);
                if (context.User.Claims.Any((c) => claimRegex.IsMatch(c.Type)))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return "RegexClaimsAuthorizationRequirement:Claim.Type=" + ClaimRegEx;
        }
    }
}
