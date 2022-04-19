using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules.Roles.Endpoints;

namespace NetEvent.Server.Modules.Roles
{
    public class RolesModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/roles", GetRoles.Handle);
            //endpoints.MapGet("/roles/{id}", GetUser.Handle);
            //endpoints.MapPut("/roles/{id}", PutUser.Handle);
            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection builder)
        {
            return builder;
        }

        public void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
