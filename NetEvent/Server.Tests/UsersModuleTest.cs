using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Data;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class UsersModuleTest
    {
        [Fact]
        public async Task UsersModuleTest_GetUsersRoute_Test()
        {
            var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptors = services.Where(a=> a.ServiceType.Name.Contains("DbContext")).ToList();

                    foreach(var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            var client = application.CreateClient();

            var users = await client.GetAsync("/api/users");

            users.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UsersModuleTest_GetUserRoute_Test()
        {
            var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptors = services.Where(a => a.ServiceType.Name.Contains("DbContext")).ToList();

                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            var userFaker = Fakers.ApplicationUserFaker();

            var user = userFaker.Generate();

            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var context = provider.GetRequiredService<ApplicationDbContext>())
                {
                    await context.Users.AddAsync(user).ConfigureAwait(false);

                    context.SaveChanges();
                }
            }

            var client = application.CreateClient();

            var users = await client.GetAsync($"/api/users/{user.Id}");

            users.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UsersModuleTest_PutUserRoute_Test()
        {
            var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptors = services.Where(a => a.ServiceType.Name.Contains("DbContext")).ToList();

                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            var applicationUserFaker = Fakers.ApplicationUserFaker();
            var userFaker = Fakers.UserFaker();

            var applicationUser = applicationUserFaker.Generate();

            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var context = provider.GetRequiredService<ApplicationDbContext>())
                {
                    await context.Users.AddAsync(applicationUser).ConfigureAwait(false);

                    context.SaveChanges();
                }
            }

            var client = application.CreateClient();

            var user = userFaker.Generate();

            user.Id = applicationUser.Id;


            var users = await client.PutAsync($"/api/users/{user.Id}", JsonContent.Create(user));

            users.EnsureSuccessStatusCode();
        }
    }
}
