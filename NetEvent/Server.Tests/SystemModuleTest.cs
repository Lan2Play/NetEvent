using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared.Config;
using NetEvent.Shared.Dto;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class SystemModuleTest : ModuleTestBase
    {
        [Fact]
        public async Task GetOrganizationHandler_Success_Test()
        {
            // Arrange
            var testData = new[]
            {
                new SystemSettingValue { Key = "key",  SerializedValue = "value" },
                new SystemSettingValue { Key = "key2", SerializedValue = "value2" }
            };

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.SystemSettingValues.AddRangeAsync(testData);
                dbContext.SaveChanges();
            }

            // Act
            var response = await Client.GetFromJsonAsync<List<SystemSettingValueDto>>($"/api/system/{SystemSettingGroup.OrganizationData}/all");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response?.Count);
            Assert.Equal(testData[0].Key, response?[0].Key);
            Assert.Equal(testData[0].SerializedValue, response?[0].Value);
            Assert.Equal(testData[1].Key, response?[1].Key);
            Assert.Equal(testData[1].SerializedValue, response?[1].Value);
        }

        [Fact]
        public async Task PostOrganizationHandler_Success_Test()
        {
            // Insert
            var organizationDataCreate = new SystemSettingValueDto("key", "value");

            var responseCreate = await Client.PostAsync($"/api/system/{SystemSettingGroup.OrganizationData}", JsonContent.Create(organizationDataCreate));

            responseCreate.EnsureSuccessStatusCode();

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var databaseOrganizationDataCreate = await dbContext.FindAsync<SystemSettingValue>(organizationDataCreate.Key).ConfigureAwait(false);

                Assert.Equal("key", databaseOrganizationDataCreate?.Key);
                Assert.Equal("value", databaseOrganizationDataCreate?.SerializedValue);
            }

            // Update value
            var organizationDataUpdate = new SystemSettingValueDto("key", "value2");

            var responseUpdate = await Client.PostAsync($"/api/system/{SystemSettingGroup.OrganizationData}", JsonContent.Create(organizationDataUpdate));

            responseUpdate.EnsureSuccessStatusCode();

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var databaseOrganizationDataUpdate = await dbContext.FindAsync<SystemSettingValue>(organizationDataUpdate.Key).ConfigureAwait(false);

                Assert.Equal("key", databaseOrganizationDataUpdate?.Key);
                Assert.Equal("value2", databaseOrganizationDataUpdate?.SerializedValue);
            }
        }

        [Fact]
        public async Task PostOrganizationHandler_Error_Test()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

            var responseCreate = await Client.PostAsync($"/api/system/{SystemSettingGroup.OrganizationData}", JsonContent.Create(new SystemSettingValueDto(null, "value")));
            Assert.False(responseCreate.IsSuccessStatusCode);
            var responseUpdate = await Client.PostAsync($"/api/system/{SystemSettingGroup.OrganizationData}", JsonContent.Create(new SystemSettingValueDto("key", null)));
            Assert.False(responseUpdate.IsSuccessStatusCode);

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
