using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto;
using NetEvent.TestHelper;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class RolesModuleTest : ModuleTestBase
    {
        [Fact]
        public async Task RolesModuleTest_GetRolesRoute_Test()
        {
            // Arrange
            var roleFaker = Fakers.ApplicationRoleFaker();

            var roleCount = 5;

            var fakeRoles = roleFaker.Generate(roleCount);

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Roles.AddRangeAsync(fakeRoles);
                dbContext.SaveChanges();
            }

            // Act
            var roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");

            // Assert
            Assert.NotNull(roles);
            Assert.Equal(roleCount, roles?.Count);
        }

        [Fact]
        public async Task RolesModuleTest_PutRolesRoute_Test()
        {
            // Arrange
            var roleFaker = Fakers.ApplicationRoleFaker();

            var roleCount = 5;

            var fakeRoles = roleFaker.Generate(roleCount);

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Roles.AddRangeAsync(fakeRoles);
                dbContext.SaveChanges();
            }

            // Act
            var roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");

            // Assert
            Assert.NotNull(roles);
            Assert.Equal(roleCount, roles?.Count);

            var role = roles?[0];
            if (role == null)
            {
                Assert.NotNull(role);
                return;
            }

            Assert.Equal(0, roles?[0]?.Claims?.Count());

            /*** Add Claims ***/
            role.Claims = new[] { "TestClaim1", "TestClaim2" };
            var result = await Client.PutAsJsonAsync($"/api/roles/{role.Id}", role);
            result.EnsureSuccessStatusCode();
            roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");
            Assert.Equal(2, roles?[0]?.Claims?.Count());

            /*** Remove Claims ***/
            role.Claims = new[] { "TestClaim1" };
            result = await Client.PutAsJsonAsync($"/api/roles/{role.Id}", role);
            result.EnsureSuccessStatusCode();
            roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");
            Assert.Equal(1, roles?[0]?.Claims?.Count());
        }

        [Fact]
        public async Task RolesModuleTest_PostRoleRoute_Test()
        {
            // Arrange
            var claimCount = 5;
            var roleFaker = Fakers.RoleFaker(claimCount);

            var fakeRole = roleFaker.Generate();

            var postRole = await Client.PostAsJsonAsync("/api/roles", fakeRole);
            postRole.EnsureSuccessStatusCode();

            // Act
            var loadedRoles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");

            // Assert
            Assert.NotNull(loadedRoles);
            Assert.Equal(1, loadedRoles?.Count);

            var claims = loadedRoles?[0].Claims;
            Assert.NotNull(claims);
            if (claims != null)
            {
                Assert.Equal(claimCount, claims.Count());
            }
        }

        [Fact]
        public async Task RolesModuleTest_PostExistingRole_Test()
        {
            // Arrange
            var claimCount = 5;
            var roleFaker = Fakers.RoleFaker(claimCount);

            var fakeRole = roleFaker.Generate();

            var postRole = await Client.PostAsJsonAsync("/api/roles", fakeRole);
            postRole.EnsureSuccessStatusCode();

            postRole = await Client.PostAsJsonAsync("/api/roles", fakeRole);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, postRole.StatusCode);
        }

        [Fact]
        public async Task RolesModuleTest_DeleteRoleRoute_Test()
        {
            // Arrange
            var roleFaker = Fakers.ApplicationRoleFaker();
            var claimFaker = Fakers.ClaimFaker();

            var roleCount = 5;

            var fakeRoles = roleFaker.Generate(roleCount);
            var fakeClaims = claimFaker.Generate(roleCount);

            using (var scope = Application.Services.CreateScope())
            {
                using var roleManager = scope.ServiceProvider.GetRequiredService<NetEventRoleManager>();
                foreach (var role in fakeRoles)
                {
                    await roleManager.CreateAsync(role);
                    foreach (var claim in fakeClaims)
                    {
                        await roleManager.AddClaimAsync(role, claim.ToClaim());
                    }
                }
            }

            // Act
            var roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");

            // Assert
            Assert.NotNull(roles);
            Assert.Equal(roleCount, roles?.Count);

            var response = await Client.DeleteAsync($"api/roles/{roles?[0].Id}");
            response.EnsureSuccessStatusCode();

            roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");

            // Assert
            Assert.NotNull(roles);
            Assert.Equal(roleCount - 1, roles?.Count);
        }
    }
}
