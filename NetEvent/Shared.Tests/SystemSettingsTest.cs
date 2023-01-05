using System.Linq;
using System.Diagnostics.CodeAnalysis;
using NetEvent.Shared.Config;
using Xunit;

namespace NetEvent.Shared.Tests
{
    [ExcludeFromCodeCoverage]
    public class SystemSettingsTest
    {
        [Fact]
        public void SystemSettingsTest_GetSettingLabelKeys_Test()
        {
            var labels = SystemSettings.GetSettingLabelKeys(SystemSettings.Instance.SettingsGroups.SelectMany(x => x.Settings.Select(s => s.Key)));
            Assert.NotNull(labels);
            Assert.NotEmpty(labels);
        }
    }
}
