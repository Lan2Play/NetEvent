﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace NetEvent.Shared.Policy
{
    public static class Policies
    {
        public static string[] AvailablePolicies { get; } = LoadAllPolicies();

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
            var result = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
            result.Requirements.Add(new RegexClaimsAuthorizationRequirement(claim));

            return result.Build();
        }

        private static AuthorizationOptions AddPolicy(this AuthorizationOptions options, string policyName)
        {
            var policyParts = policyName.Split('.');
            if (policyParts.Length > 1)
            {
                for (int i = 1; i < policyParts.Length; i++)
                {
                    var regexName = $"{string.Join('.', policyParts.Take(i))}.*";
                    options.AddPolicy(regexName, HasClaim(regexName));
                }
            }

            options.AddPolicy(policyName, HasClaim(policyName));
            return options;
        }

        private static string[] LoadAllPolicies()
        {
            return LoadAllPolicies(typeof(Policy)).ToArray();
        }

        private static IEnumerable<string> LoadAllPolicies(Type parent)
        {
            foreach (var member in parent.GetFields())
            {
                yield return member.GetValue(null)!.ToString()!;
            }
        }
    }
}
