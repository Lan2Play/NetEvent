using System;
using System.Diagnostics.CodeAnalysis;
using NetEvent.Shared.Policy;
using Xunit;

namespace NetEvent.Shared.Tests
{
    [ExcludeFromCodeCoverage]
    public class RegexClaimsAuthorizationRequirementTest
    {
        [Fact]
        public void RegexClaimsAuthorizationRequirementTest_Test()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => new RegexClaimsAuthorizationRequirement(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var regexClaimsAuthorizationRequirement = new RegexClaimsAuthorizationRequirement("Test");
            Assert.NotNull(regexClaimsAuthorizationRequirement.ToString());
        }
    }
}
