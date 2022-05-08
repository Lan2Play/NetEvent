﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
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
            var roleFaker = Fakers.IdentityRoleFaker();

            var roleCount = 5;

            var fakeRoles = roleFaker.Generate(roleCount);

            using (var scope = Application.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Roles.AddRangeAsync(fakeRoles).ConfigureAwait(false);
                dbContext.SaveChanges();
            }

            // Act
            var roles = await Client.GetFromJsonAsync<List<RoleDto>>("/api/roles");

            // Assert
            Assert.NotNull(roles);
            Assert.Equal(roleCount, roles?.Count);
        }
    }
}