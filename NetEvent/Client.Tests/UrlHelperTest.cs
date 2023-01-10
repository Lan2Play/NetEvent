using System.Diagnostics.CodeAnalysis;
using NetEvent.Client.Extensions;
using Xunit;

namespace NetEvent.Client.Tests
{
    [ExcludeFromCodeCoverage]
    public class UrlHelperTest
    {
        [Fact]
        public void UrlHelperTest_GetEventLink_Test()
        {
            Assert.Equal("/event/2", UrlHelper.GetEventLink(2, false));
            Assert.Equal("/administration/event/2", UrlHelper.GetEventLink(2, true));
        }

        [Fact]
        public void UrlHelperTest_GetVenueLink_Test()
        {
            Assert.Equal("/venue/2", UrlHelper.GetVenueLink(2, false));
            Assert.Equal("/administration/venue/2", UrlHelper.GetVenueLink(2, true));
        }
    }
}
