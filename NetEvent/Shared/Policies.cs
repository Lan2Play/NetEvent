using Microsoft.AspNetCore.Authorization;

namespace NetEvent.Shared
{
    public static class Policies
    {
        public static string[] AvailablePolicies { get; } = new string[]
        {
            "Admin.Users.Read",
            "Admin.Users.Edit"
        };

        public static AuthorizationOptions AddPolicies(this AuthorizationOptions options)
        {
            foreach (var availablePolicy in AvailablePolicies)
            {
                options.AddPolicy(availablePolicy);
            }

            return options;
        }

        private static AuthorizationPolicy HasClaim(string claim)
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireClaim(claim)
                                                   .Build();
        }

        private static AuthorizationOptions AddPolicy(this AuthorizationOptions options, string policyName)
        {
            options.AddPolicy(policyName, HasClaim(policyName));
            return options;
        }
    }
}
