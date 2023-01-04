using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Moq;
using NetEvent.Client.Services;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared.Policy;
using Xunit;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    public class ModuleTestBase : IDisposable
    {
        internal WebApplicationFactory<Program> Application { get; }

        protected System.Net.Http.HttpClient Client { get; }

        private bool disposedValue;

        protected ModuleTestBase()
        {
            var dbName = $"{nameof(ModuleTestBase)}_{Guid.NewGuid()}";
            Application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptors = services.Where(a => a.ServiceType.Name.Contains("DbContext", StringComparison.OrdinalIgnoreCase)).ToList();

                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });

                    services.AddScoped<NetEventAuthenticationStateProvider>();
                    services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<NetEventAuthenticationStateProvider>());
                    services.AddScoped<IAuthService, AuthService>();

                    var mockClientFactory = new Mock<IHttpClientFactory>();
                    mockClientFactory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(() => Client!);
                    services.TryAddSingleton(mockClientFactory.Object);
                    services.Configure<HttpClientFactoryOptions>(Constants.BackendApiSecuredHttpClientName, o =>
                    {
                        o.HttpMessageHandlerBuilderActions.Add(b => b.AdditionalHandlers.Add(b.Services.GetRequiredService<BaseAddressAuthorizationMessageHandler>()));
                    });
                });
            });

            Client = Application.CreateClient();
        }

        protected static async Task AuthenticatedClient(IServiceScope scope)
        {
            // Arrange
            var userFaker = Fakers.ApplicationUserFaker();
            var roleFaker = Fakers.ApplicationRoleFaker();
            const string password = "Test123..";

            var fakeUser = userFaker.Generate();
            var fakeRole = roleFaker.Generate();
            fakeUser.EmailConfirmed = true;
            fakeRole.IsDefault = true;

            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var roleResult = await roleManager.CreateAsync(fakeRole).ConfigureAwait(false);
            Assert.True(roleResult?.Succeeded);
            foreach (var policy in Policies.AvailablePolicies)
            {
                var addClaimsResult = await roleManager.AddClaimAsync(fakeRole, new Claim(policy, string.Empty));
                Assert.True(addClaimsResult?.Succeeded);
            }

            var userResult = await userManager.CreateAsync(fakeUser, password).ConfigureAwait(false);
            Assert.True(userResult?.Succeeded);

            var userRoleResult = await userManager.AddToRoleAsync(fakeUser, fakeRole.NormalizedName!).ConfigureAwait(false);
            Assert.True(userRoleResult?.Succeeded);

            var authManager = scope.ServiceProvider.GetRequiredService<NetEventAuthenticationStateProvider>();
            var loginResult = await authManager.Login(new Shared.Dto.LoginRequestDto { UserName = fakeUser.UserName!, Password = password }).ConfigureAwait(false);
            Assert.True(loginResult?.Successful);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client?.Dispose();
                    Application?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
