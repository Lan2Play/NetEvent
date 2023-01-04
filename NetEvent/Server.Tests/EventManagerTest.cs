using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Client.Pages.Administration.Venues;
using NetEvent.Client.Services;
using NetEvent.Server.Data.Events;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto.Event;
using NetEvent.Shared.Policy;
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
            var result = await eventManager.ValidateEventAsync(null).ConfigureAwait(false);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task EventManagerTest_DeleteMissingEvent_Test()
        {
            using var scope = Application.Services.CreateScope();
            var eventManager = scope.ServiceProvider.GetRequiredService<IEventManager>();
            var result = await eventManager.DeleteAsync(Fakers.EventFaker(Fakers.VenueFaker().Generate(2)).Generate()).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task EventManagerTest_DeleteMissingEventId_Test()
        {
            using var scope = Application.Services.CreateScope();
            var eventManager = scope.ServiceProvider.GetRequiredService<IEventManager>();
            var result = await eventManager.DeleteAsync(1337).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }
    }
}
