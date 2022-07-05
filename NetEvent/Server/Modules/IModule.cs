using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetEvent.Server.Modules
{
    public interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection builder);

        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);

        void OnModelCreating(ModelBuilder builder);
    }
}
