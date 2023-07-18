using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
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
        public async Task GetOrganizationSettingsHandler_Success_Test()
        {
            // Arrange
            var testData = new[]
            {
                new SystemSettingValue { Key = SystemSettings.OrganizationData.OrganizationName,  SerializedValue = "value" },
                new SystemSettingValue { Key = SystemSettings.OrganizationData.Logo, SerializedValue = "ImageId" }
            };

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.SystemSettingValues.AddRangeAsync(testData);
                dbContext.SaveChanges();
            }

            // Act
            var response = await Client.GetFromJsonAsync<List<SystemSettingValueDto>>($"/api/system/settings/{SystemSettingGroup.OrganizationData}/all");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response?.Count);
            Assert.All(response!, x => Assert.True(new SystemSettings.OrganizationData().Settings.First(s => s.Key.Equals(x.Key, StringComparison.OrdinalIgnoreCase)).ValueType.IsValid(x.Value)));
            Assert.Equal(testData[0].Key, response?[0].Key);
            Assert.Equal(testData[0].SerializedValue, response?[0].Value);
            Assert.Equal(testData[1].Key, response?[1].Key);
            Assert.Equal(testData[1].SerializedValue, response?[1].Value);
        }

        [Fact]
        public async Task GetOrganizationSettingHandler_Success_Test()
        {
            // Arrange
            var testData = new[]
            {
                new SystemSettingValue { Key = SystemSettings.OrganizationData.OrganizationName,  SerializedValue = "value" },
                new SystemSettingValue { Key = SystemSettings.OrganizationData.Logo, SerializedValue = "ImageId" }
            };

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.SystemSettingValues.AddRangeAsync(testData);
                dbContext.SaveChanges();
            }

            // Act
            var response = await Client.GetFromJsonAsync<SystemSettingValueDto>($"/api/system/settings/{SystemSettingGroup.OrganizationData}/{SystemSettings.OrganizationData.Logo}");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(testData[1].Key, response?.Key);
            Assert.Equal(testData[1].SerializedValue, response?.Value);
        }

        [Fact]
        public async Task GetAuthenticationSettingsHandler_Success_Test()
        {
            // Arrange
            var testData = new[]
            {
                new SystemSettingValue { Key = SystemSettings.AuthenticationData.Standard,  SerializedValue = "True" },
                new SystemSettingValue { Key = SystemSettings.AuthenticationData.Steam, SerializedValue = "False" }
            };

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.SystemSettingValues.AddRangeAsync(testData);
                dbContext.SaveChanges();
            }

            // Act
            var response = await Client.GetFromJsonAsync<List<SystemSettingValueDto>>($"/api/system/settings/{SystemSettingGroup.AuthenticationData}/all");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response?.Count);
            Assert.All(response!, x => Assert.True(new SystemSettings.AuthenticationData().Settings.First(s => s.Key.Equals(x.Key, StringComparison.OrdinalIgnoreCase)).ValueType.IsValid(x.Value)));
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

            var responseCreate = await Client.PostAsync($"/api/system/settings/{SystemSettingGroup.OrganizationData}", JsonContent.Create(organizationDataCreate));

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

            var responseUpdate = await Client.PostAsync($"/api/system/settings/{SystemSettingGroup.OrganizationData}", JsonContent.Create(organizationDataUpdate));

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

            var responseCreate = await Client.PostAsync($"/api/system/settings/{SystemSettingGroup.OrganizationData}", JsonContent.Create(new SystemSettingValueDto(null, "value")));
            Assert.False(responseCreate.IsSuccessStatusCode);

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public async Task GetSystemInfoVersionsSetted_Test()
        {
            // Arrange
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Environment.SetEnvironmentVariable("BUILDNODE", "TEST");
            Environment.SetEnvironmentVariable("BUILDID", "TEST");
            Environment.SetEnvironmentVariable("BUILDNUMBER", "TEST");
            Environment.SetEnvironmentVariable("SOURCE_COMMIT", "TEST");

            // Act
            var response = await Client.GetFromJsonAsync<SystemInfoDto>($"/api/system/info/all");

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response?.Components);
            Assert.Equal(currentDomain.GetAssemblies().Length, response?.Components.Count);
            Assert.NotNull(response?.Health);
            Assert.NotNull(response?.Versions);
            Assert.NotEmpty(response!.Health);
            Assert.NotEmpty(response!.Versions);
            Assert.Equal("TEST", response?.Versions?.FirstOrDefault(x => x.Component.Equals("BUILDNODE", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal("TEST", response?.Versions?.FirstOrDefault(x => x.Component.Equals("BUILDID", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal("TEST", response?.Versions?.FirstOrDefault(x => x.Component.Equals("BUILDNUMBER", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal("TEST", response?.Versions?.FirstOrDefault(x => x.Component.Equals("SOURCE_COMMIT", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal(Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion, response?.Versions?.FirstOrDefault(x => x.Component.Equals("NETEVENT", StringComparison.OrdinalIgnoreCase))?.Version);
        }

        [Fact]
        public async Task GetSystemInfoVersionsNotSetted_Test()
        {
            // Arrange
            Environment.SetEnvironmentVariable("BUILDNODE", string.Empty);
            Environment.SetEnvironmentVariable("BUILDID", string.Empty);
            Environment.SetEnvironmentVariable("BUILDNUMBER", string.Empty);
            Environment.SetEnvironmentVariable("SOURCE_COMMIT", string.Empty);

            // Act
            var response = await Client.GetFromJsonAsync<SystemInfoDto>($"/api/system/info/all");

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response?.Versions);
            Assert.NotEmpty(response!.Versions);
            Assert.NotEqual(0, response?.Versions.Count);
            Assert.Equal("dev", response?.Versions?.FirstOrDefault(x => x.Component.Equals("BUILDNODE", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal("dev", response?.Versions?.FirstOrDefault(x => x.Component.Equals("BUILDID", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal("dev", response?.Versions?.FirstOrDefault(x => x.Component.Equals("BUILDNUMBER", StringComparison.OrdinalIgnoreCase))?.Version);
            Assert.Equal("dev", response?.Versions?.FirstOrDefault(x => x.Component.Equals("SOURCE_COMMIT", StringComparison.OrdinalIgnoreCase))?.Version);
        }

        [Fact]
        public async Task PostSystemImageHandler_Error_Test()
        {
            var jsonResult = await Client.PostAsync("/api/system/image/testImage", JsonContent.Create("egal"));
            Assert.Equal(HttpStatusCode.BadRequest, jsonResult.StatusCode);

            using var multipartFormContent = new MultipartFormDataContent();
            var responseCreate = await Client.PostAsync("/api/system/image/testImage", multipartFormContent);
            Assert.False(responseCreate.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PostAndGetSystemImageHandler_Success_Test()
        {
            using var multipartFormContent = new MultipartFormDataContent();

            var fileStreamContent = new StreamContent(File.OpenRead("Data/Test.png"));
            fileStreamContent.Headers.ContentType = new("image/png");
            multipartFormContent.Add(fileStreamContent, name: "image", fileName: "Test.png");

            var responseCreate = await Client.PostAsync("/api/system/image/testImage", multipartFormContent);
            Assert.True(responseCreate.IsSuccessStatusCode);
            var uploadedImageId = await responseCreate.Content.ReadFromJsonAsync<string>();

            var imageFromId = await Client.GetAsync($"/api/system/image/{uploadedImageId}");
            Assert.True(imageFromId.IsSuccessStatusCode);
        }

        [Fact]
        public Task PostGetDeleteSystemImagesHandler_Success_Test()
        {
            return RunWithFakeEvents(async events =>
            {
                using var multipartFormContent = new MultipartFormDataContent();

                var fileStreamContent = new StreamContent(File.OpenRead("Data/Test.png"));
                fileStreamContent.Headers.ContentType = new("image/png");
                multipartFormContent.Add(fileStreamContent, name: "image", fileName: "Test.png");

                var responseCreate = await Client.PostAsync("/api/system/image/testImage", multipartFormContent);
                Assert.True(responseCreate.IsSuccessStatusCode);

                var images = await Client.GetFromJsonAsync<List<SystemImageWithUsagesDto>>("/api/system/image/all");
                Assert.NotNull(images);
                Assert.NotEmpty(images!);

                var response = await Client.DeleteAsync($"api/system/image/{images![0].Image.Id}");
                response.EnsureSuccessStatusCode();

                images = await Client.GetFromJsonAsync<List<SystemImageWithUsagesDto>>("/api/system/image/all");
                Assert.NotNull(images);
                Assert.Empty(images!);
            });
        }

        [Fact]
        public async Task GetNetEventStyle_Success_Test()
        {
            // Arrange
            var testData = new[]
            {
                new SystemSettingValue { Key = SystemSettings.StyleData.PrimaryColor,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.PrimaryTextColor,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.SecondaryColor,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.SecondaryTextColor,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.Background,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.AppbarBackground,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.AppbarText,  SerializedValue = "#123456" },
                new SystemSettingValue { Key = SystemSettings.StyleData.CustomCss,  SerializedValue = "test {background: red;}" },
            };

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.SystemSettingValues.AddRangeAsync(testData);
                dbContext.SaveChanges();
            }

            // Act
            var response = await Client.GetStringAsync($"/css/netevent.css");

            // Assert
            Assert.NotNull(response);
        }
    }
}
