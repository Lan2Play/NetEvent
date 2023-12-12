using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NetEvent.Client.Services
{
    public static class NavigationServiceExtension
    {
        public static Task InitializeNavigationService(this WebAssemblyHost app)
        {
            var navigationService = app.Services.GetRequiredService<NavigationService>();
            return navigationService == null
                ? throw new NotSupportedException("Start without NavigationServce is not possible!")
                : Task.CompletedTask;
        }
    }
}
