using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class OrganizationModuleTest : ModuleTestBase
    {
        [Fact]
        public async Task GetOrganizationHandler_Success_Test()
        {
            // Arrange
            var testData = new[]
            {
                new OrganizationData{ Key = "key", Value = "value" },
                new OrganizationData{ Key = "key2", Value = "value2" }
            };

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.OrganizationData.AddRangeAsync(testData);
                dbContext.SaveChanges();
            }

            // Act
            var response = await Client.GetFromJsonAsync<List<OrganizationDataDto>>("/api/organization/all");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response?.Count);
            Assert.Equal(testData[0].Key, response?[0].Key);
            Assert.Equal(testData[0].Value, response?[0].Value);
            Assert.Equal(testData[1].Key, response?[1].Key);
            Assert.Equal(testData[1].Value, response?[1].Value);
        }

        [Fact]
        public async Task PostOrganizationHandler_Success_Test()
        {
            // Insert
            var organizationDataCreate = new OrganizationDataDto("key", "value");

            var responseCreate = await Client.PostAsync("/api/organization", JsonContent.Create(organizationDataCreate));

            responseCreate.EnsureSuccessStatusCode();

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var databaseOrganizationDataCreate = await dbContext.FindAsync<OrganizationData>(organizationDataCreate.Key).ConfigureAwait(false);

                Assert.Equal("key", databaseOrganizationDataCreate?.Key);
                Assert.Equal("value", databaseOrganizationDataCreate?.Value);
            }

            // Update value
            var organizationDataUpdate = new OrganizationDataDto("key", "value2");

            var responseUpdate = await Client.PostAsync("/api/organization", JsonContent.Create(organizationDataUpdate));

            responseUpdate.EnsureSuccessStatusCode();

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var databaseOrganizationDataUpdate = await dbContext.FindAsync<OrganizationData>(organizationDataUpdate.Key).ConfigureAwait(false);

                Assert.Equal("key", databaseOrganizationDataUpdate?.Key);
                Assert.Equal("value2", databaseOrganizationDataUpdate?.Value);
            }
        }

        [Fact]
        public async Task PostOrganizationHandler_Error_Test()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

            var responseCreate = await Client.PostAsync("/api/organization", JsonContent.Create(new OrganizationDataDto(null, "value")));
            Assert.False(responseCreate.IsSuccessStatusCode);
            var responseUpdate = await Client.PostAsync("/api/organization", JsonContent.Create(new OrganizationDataDto("key", null)));
            Assert.False(responseUpdate.IsSuccessStatusCode);

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
