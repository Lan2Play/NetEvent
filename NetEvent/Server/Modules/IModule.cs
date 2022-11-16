using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetEvent.Server.Modules
{
    public interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection builder);

        void OnModelCreating(ModelBuilder builder);

        /// <summary>
        /// Map routes in root Context
        /// </summary>
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);

        /// <summary>
        /// Map routes in module context /api/admin/[moduleName]
        /// </summary>
        IEndpointRouteBuilder MapModuleEndpoints(IEndpointRouteBuilder endpoints);

        /// <summary>
        /// Map routes in module context /api/read/[moduleName]
        /// These are routes that require [moduleName]Read Permission
        /// </summary>
        IEndpointRouteBuilder MapModuleReadAuthEndpoints(IEndpointRouteBuilder endpoints);

        /// <summary>
        /// Map routes in module context /api/write/[moduleName]
        /// These are routes that require [moduleName]Write Permission
        /// </summary>
        IEndpointRouteBuilder MapModuleWriteAuthEndpoints(IEndpointRouteBuilder endpoints);
    }
}
