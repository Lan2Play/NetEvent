using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class UsersModuleTest : ModuleTestBase
    {
        [Fact]
        public async Task UsersModuleTest_GetUsersRoute_Test()
        {
            // Arrange
            var userFaker = Fakers.ApplicationUserFaker();

            var usersCount = 5;

            var fakeUsers = userFaker.Generate(usersCount);

            await DbContext.Users.AddRangeAsync(fakeUsers).ConfigureAwait(false);

            DbContext.SaveChanges();

            // Act
            var users = await Client.GetFromJsonAsync<List<UserDto>>("/api/users");

            // Assert
            Assert.NotNull(users);
            Assert.Equal(usersCount, users?.Count);
        }

        [Fact]
        public async Task UsersModuleTest_GetUser_Success_Test()
        {
            // Arrange
            var userFaker = Fakers.ApplicationUserFaker();

            var fakeUser = userFaker.Generate();

            await DbContext.Users.AddAsync(fakeUser).ConfigureAwait(false);

            DbContext.SaveChanges();
           
            // Act
            var user = await Client.GetFromJsonAsync<UserDto>($"/api/users/{fakeUser.Id}");

            // Assert
            Assert.NotNull(user);
            Assert.Equal(fakeUser.Id, user?.Id);
            Assert.Equal(fakeUser.FirstName, user?.FirstName);
            Assert.Equal(fakeUser.LastName, user?.LastName);
        }

        [Fact]
        public async Task UsersModuleTest_GetUser_NotFound_Test()
        {
            // Act
            var responseMessage = await Client.GetAsync($"/api/users/notexistinguser");

            // Assert
            Assert.NotNull(responseMessage);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, responseMessage.StatusCode);           
        }

        [Fact]
        public async Task UsersModuleTest_PutUserRoute_Test()
        {
            var applicationUserFaker = Fakers.ApplicationUserFaker();
            var userFaker = Fakers.UserFaker();

            var applicationUser = applicationUserFaker.Generate();

            await DbContext.Users.AddAsync(applicationUser).ConfigureAwait(false);

            DbContext.SaveChanges();

            var fakeUser = userFaker.Generate();
            fakeUser.Id = applicationUser.Id;

            //Act
            var response = await Client.PutAsJsonAsync($"/api/users/{fakeUser.Id}", fakeUser);

            response.EnsureSuccessStatusCode();

            var databaseUser = await DbContext.FindAsync<ApplicationUser>(applicationUser.Id).ConfigureAwait(false);

            //Assert
            Assert.Equal(fakeUser.FirstName, databaseUser?.FirstName);
            Assert.Equal(fakeUser.LastName, databaseUser?.LastName);
            Assert.Equal(fakeUser.EmailConfirmed, databaseUser?.EmailConfirmed);
            Assert.Equal(fakeUser.UserName, databaseUser?.UserName);
        }
    }
}
