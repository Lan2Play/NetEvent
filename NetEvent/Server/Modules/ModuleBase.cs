using Microsoft.EntityFrameworkCore;

namespace NetEvent.Server.Modules
{
    public abstract class ModuleBase : IModule
    {
        public abstract IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
        public abstract IServiceCollection RegisterModule(IServiceCollection builder);

        public abstract void OnModelCreating(ModelBuilder builder);

        protected IResult ToApiResult<T>(ResponseBase<T> response)
        {
            switch (response.ReturnType)
            {
                case ReturnType.Ok:
                    return Results.Ok(response.ReturnValue);
                case ReturnType.NotFound:
                    return Results.NotFound();
                case ReturnType.Error:
                    return Results.BadRequest(response.Error);
                default:
                    throw new NotSupportedException($"ReturnType {response.ReturnType} is not supported!");
            }
        }
    }
}
