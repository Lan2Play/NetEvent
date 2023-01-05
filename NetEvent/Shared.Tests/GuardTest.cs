using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace NetEvent.Shared.Tests
{
    [ExcludeFromCodeCoverage]
    public class GuardTest
    {
        [Fact]
        public void GuardTest_IsNotNull_Test()
        {
            Guard.IsNotNull(new object(), string.Empty);
            Guard.IsNotNull(string.Empty, string.Empty);
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull(null, string.Empty));
        }

        [Fact]
        public void GuardTest_IsNotNullOrEmpty_Test()
        {
            Guard.IsNotNullOrEmpty("Test", string.Empty);
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullOrEmpty(null, string.Empty));
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullOrEmpty(string.Empty, string.Empty));
        }
    }
}
