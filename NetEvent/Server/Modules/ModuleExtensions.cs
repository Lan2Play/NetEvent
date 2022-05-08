using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NetEvent.Server.Modules
{
    public static class ModuleExtensions
    {
        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            var modules = DiscoverModules();

            var moduleList = new List<IModule>();

            foreach (var module in modules)
            {
                module.RegisterModule(services);
                moduleList.Add(module);
            }

            services.AddSingleton<IReadOnlyCollection<IModule>>(moduleList);

            return services;
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
            var registeredModules = app.Services.GetRequiredService<IReadOnlyCollection<IModule>>();

            foreach (var module in registeredModules)
            {
                module.MapEndpoints(app);
            }

            return app;
        }

        private static IEnumerable<IModule> DiscoverModules()
        {
            return typeof(IModule).Assembly
                .GetTypes()
                .Where(p => p.IsClass && !p.IsAbstract && p.IsAssignableTo(typeof(IModule)))
                .Select(Activator.CreateInstance)
                .Cast<IModule>();
        }
    }
}
