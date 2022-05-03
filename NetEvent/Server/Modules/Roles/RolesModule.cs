using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Modules.Roles.Endpoints;

namespace NetEvent.Server.Modules.Roles
{
    [ExcludeFromCodeCoverage]
    public class RolesModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/roles", GetRoles.Handle);
            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection builder)
        {
            return builder;
        }

        public void OnModelCreating(ModelBuilder builder)
        {
            // No need to modify database scheme
        }
    }
}
