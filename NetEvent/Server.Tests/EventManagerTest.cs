using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data.Events;
using NetEvent.TestHelper;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class EventManagerTest : ModuleTestBase
    {
        [Fact]
        public async Task EventManagerTest_ValidateEventNull_Test()
        {
            using var scope = Application.Services.CreateScope();
            var eventManager = scope.ServiceProvider.GetRequiredService<IEventManager>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var result = await eventManager.ValidateEventAsync(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task EventManagerTest_DeleteMissingEvent_Test()
        {
            using var scope = Application.Services.CreateScope();
            var eventManager = scope.ServiceProvider.GetRequiredService<IEventManager>();
            var result = await eventManager.DeleteAsync(Fakers.EventFaker(Fakers.VenueFaker().Generate(2)).Generate());

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task EventManagerTest_DeleteMissingEventId_Test()
        {
            using var scope = Application.Services.CreateScope();
            var eventManager = scope.ServiceProvider.GetRequiredService<IEventManager>();
            var result = await eventManager.DeleteAsync(1337);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }
    }
}
