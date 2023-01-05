using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data.Events;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto.Event;
using NetEvent.TestHelper;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class VenueModuleTest : ModuleTestBase
    {
        [Fact]
        public Task VenueModuleTest_GetVenuesRoute_Test()
        {
            // Arrange
            return RunWithFakeVenues(async fakeVenues =>
            {
                // Act
                var venues = await Client.GetFromJsonAsync<IEnumerable<VenueDto>>("/api/venues").ConfigureAwait(false);

                // Assert
                Assert.NotNull(venues);
                Assert.Equal(venues.Count(), fakeVenues.Count);
            });
        }

        [Fact]
        public Task VenueModuleTest_GetVenueByIdRoute_Test()
        {
            // Arrange
            return RunWithFakeVenues(async fakeVenues =>
            {
                // Act
                var venueById = await Client.GetFromJsonAsync<VenueDto>($"/api/venues/{fakeVenues[0].Id}").ConfigureAwait(false);

                // Assert
                Assert.NotNull(venueById);
                Assert.Equal(venueById.Name, fakeVenues[0].Name);
            });
        }

        [Fact]
        public Task VenueModuleTest_GetVenueBySlugRoute_Test()
        {
            // Arrange
            return RunWithFakeVenues(async fakeVenues =>
            {
                // Act
                var venueBySlug = await Client.GetFromJsonAsync<VenueDto>($"/api/venues/{fakeVenues[0].Slug}").ConfigureAwait(false);

                // Assert
                Assert.NotNull(venueBySlug);
                Assert.Equal(venueBySlug.Name, fakeVenues[0].Name);
            });
        }

        [Fact]
        public Task VenueModuleTest_PostVenueRoute_Test()
        {
            // Arrange
            return RunWithFakeVenues(async fakeVenues =>
            {
                var fakeVenue = Fakers.VenueFaker().Generate();

                // Act
                var postResult = await Client.PostAsJsonAsync($"/api/venues/", fakeVenue).ConfigureAwait(false);
                postResult.EnsureSuccessStatusCode();
                var venues = await Client.GetFromJsonAsync<IEnumerable<VenueDto>>("/api/venues").ConfigureAwait(false);

                // Assert
                Assert.NotNull(venues);
                Assert.Equal(venues.Count(), fakeVenues.Count + 1);
            },
            true);
        }

        [Fact]
        public Task VenueModuleTest_PutVenueRoute_Test()
        {
            // Arrange
            return RunWithFakeVenues(async fakeVenues =>
            {
                var faker = new Faker();
                var fakeVenue = fakeVenues[0];
                fakeVenue.Name = faker.Name.JobTitle();

                // Act
                var postResult = await Client.PutAsJsonAsync($"/api/venues/{fakeVenue.Id}", fakeVenue).ConfigureAwait(false);
                postResult.EnsureSuccessStatusCode();
                var updatedVenue = await Client.GetFromJsonAsync<VenueDto>($"/api/venues/{fakeVenue.Id}").ConfigureAwait(false);

                // Assert
                Assert.NotNull(updatedVenue);
                Assert.Equal(fakeVenue.Name, updatedVenue.Name);
            },
            true);
        }

        [Fact]
        public Task VenueModuleTest_DeleteVenueRoute_Test()
        {
            // Arrange
            return RunWithFakeVenues(async fakeVenues =>
            {
                var fakeVenue = fakeVenues[0];

                // Act
                var postResult = await Client.DeleteAsync($"/api/venues/{fakeVenue.Id}").ConfigureAwait(false);
                postResult.EnsureSuccessStatusCode();
                await Assert.ThrowsAsync<HttpRequestException>(async () => await Client.GetFromJsonAsync<VenueDto>($"/api/venues/{fakeVenue.Id}").ConfigureAwait(false)).ConfigureAwait(false);
                var venues = await Client.GetFromJsonAsync<IEnumerable<VenueDto>>("/api/venues").ConfigureAwait(false);

                // Assert
                Assert.NotNull(venues);
                Assert.Equal(venues.Count(), fakeVenues.Count - 1);
                Assert.DoesNotContain(venues, v => v.Id == fakeVenue.Id);
            },
            true);
        }

        private async Task RunWithFakeVenues(Func<List<Venue>, Task> action, bool auth = false)
        {
            const int fakeCount = 5;
            List<Venue> fakeVenues;

            using (var scope = Application.Services.CreateScope())
            {
                var eventManager = scope.ServiceProvider.GetRequiredService<IEventManager>();

                var venueFaker = Fakers.VenueFaker();
                fakeVenues = venueFaker.Generate(fakeCount);
                foreach (var fakeVenue in fakeVenues)
                {
                    await eventManager.CreateVenueAsync(fakeVenue).ConfigureAwait(false);
                }

                if (auth)
                {
                    await AuthenticatedClient(scope).ConfigureAwait(false);
                }
            }

            await action(fakeVenues).ConfigureAwait(false);
        }
    }
}
