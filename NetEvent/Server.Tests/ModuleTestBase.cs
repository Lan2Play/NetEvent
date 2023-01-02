using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Moq;
using NetEvent.Client.Services;
using NetEvent.Server.Data;

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
                    var descriptors = services.Where(a => a.ServiceType.Name.Contains("DbContext")).ToList();

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
