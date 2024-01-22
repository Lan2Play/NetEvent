using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using NetEvent.Shared.Dto.Event;
using NetEvent.TestHelper;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class EventModuleTest : ModuleTestBase
    {
        [Fact]
        public Task EventModuleTest_GetEventsRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                // Act
                var events = await Client.GetFromJsonAsync<IEnumerable<EventDto>>("/api/events");

                // Assert
                Assert.NotNull(events);
                Assert.Equal(events.Count(), fakeEvents.Count);
            });
        }

        [Fact]
        public Task EventModuleTest_GetUpcomingEventRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                // Act
                var upcomingEvent = await Client.GetFromJsonAsync<EventDto>("/api/events/upcoming");

                // Assert
                Assert.NotNull(upcomingEvent);
                Assert.Equal(upcomingEvent.Name, fakeEvents.OrderBy(x => x.StartDate).First().Name);
            });
        }

        [Fact]
        public Task EventModuleTest_GetEventByIdRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                // Act
                var eventById = await Client.GetFromJsonAsync<EventDto>($"/api/events/{fakeEvents[0].Id}");

                // Assert
                Assert.NotNull(eventById);
                Assert.Equal(eventById.Name, fakeEvents[0].Name);
            });
        }

        [Fact]
        public Task EventModuleTest_GetEventBySlugRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                // Act
                var eventBySlug = await Client.GetFromJsonAsync<EventDto>($"/api/events/{fakeEvents[0].Slug}");

                // Assert
                Assert.NotNull(eventBySlug);
                Assert.Equal(eventBySlug.Name, fakeEvents[0].Name);
            });
        }

        [Fact]
        public Task EventModuleTest_PostEventRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                var fakeVenue = fakeEvents[0].Venue!;
                var fakeEvent = Fakers.EventFaker(new[] { fakeVenue }).Generate();

                // Act
                var postResult = await Client.PostAsJsonAsync($"/api/events/", fakeEvent);
                postResult.EnsureSuccessStatusCode();
                var events = await Client.GetFromJsonAsync<IEnumerable<EventDto>>("/api/events");

                // Assert
                Assert.NotNull(events);
                Assert.Equal(events.Count(), fakeEvents.Count + 1);
            },
            true);
        }

        [Fact]
        public Task EventModuleTest_PutEventRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                var faker = new Faker();
                var fakeEvent = fakeEvents[0];
                fakeEvent.Name = faker.Name.JobTitle();

                // Act
                var postResult = await Client.PutAsJsonAsync($"/api/events/{fakeEvent.Id}", fakeEvent);
                postResult.EnsureSuccessStatusCode();
                var updatedEvent = await Client.GetFromJsonAsync<EventDto>($"/api/events/{fakeEvent.Id}");

                // Assert
                Assert.NotNull(updatedEvent);
                Assert.Equal(fakeEvent.Name, updatedEvent.Name);
            },
            true);
        }

        [Fact]
        public Task EventModuleTest_DeleteEventRoute_Test()
        {
            // Arrange
            return RunWithFakeEvents(async fakeEvents =>
            {
                var fakeEvent = fakeEvents[0];

                // Act
                var postResult = await Client.DeleteAsync($"/api/events/{fakeEvent.Id}");
                postResult.EnsureSuccessStatusCode();
                await Assert.ThrowsAsync<HttpRequestException>(async () => await Client.GetFromJsonAsync<EventDto>($"/api/events/{fakeEvent.Id}"));
                var events = await Client.GetFromJsonAsync<IEnumerable<EventDto>>("/api/events");

                // Assert
                Assert.NotNull(events);
                Assert.Equal(events.Count(), fakeEvents.Count - 1);
                Assert.DoesNotContain(events, v => v.Id == fakeEvent.Id);
            },
            true);
        }
    }
}
