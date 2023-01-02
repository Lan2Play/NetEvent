using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Shared.Policy;

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
                var moduleName = module.GetType().Name.Replace("Module", string.Empty, StringComparison.OrdinalIgnoreCase);

                var moduleGroup = app.MapGroup($"/api/{moduleName}");
                var readModuleGroup = app.MapGroup($"/api/{moduleName}");
                var writeModuleGroup = app.MapGroup($"/api/{moduleName}");

                module.MapEndpoints(app);

                module.MapModuleEndpoints(moduleGroup);
                module.MapModuleReadAuthEndpoints(readModuleGroup);
                module.MapModuleWriteAuthEndpoints(writeModuleGroup);

                var readPolicy = $"Admin.{moduleName}.Read";
                var writePolicy = $"Admin.{moduleName}.Write";

                if (Policies.AvailablePolicies.Contains(readPolicy, StringComparer.OrdinalIgnoreCase))
                {
                    readModuleGroup.RequireAuthorization(readPolicy);
                }

                if (Policies.AvailablePolicies.Contains(writePolicy, StringComparer.OrdinalIgnoreCase))
                {
                    writeModuleGroup.RequireAuthorization(writePolicy);
                }
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
