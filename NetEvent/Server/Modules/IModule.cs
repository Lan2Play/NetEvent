namespace NetEvent.Server.Modules
{
    public interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection builder);
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }

    public static class ModuleExtensions
    {
        // this could also be added into the DI container
        static readonly List<IModule> registeredModules = new List<IModule>();

        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            var modules = DiscoverModules();
            foreach (var module in modules)
            {
                module.RegisterModule(services);
                registeredModules.Add(module);
            }

            return services;
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
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

    public abstract class ModuleBase : IModule
    {
        public abstract IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
        public abstract IServiceCollection RegisterModule(IServiceCollection builder);

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



    public class ResponseBase<T>
    {
        public ResponseBase(T? value)
        {
            ReturnValue = value;
            ReturnType = ReturnType.Ok;
        }

        public ResponseBase(ReturnType returnType, string error)
        {
            ReturnType = returnType;
            Error = error;
        }


        public ReturnType ReturnType { get; set; }
        public string? Error { get; set; }

        public T? ReturnValue { get; set; }

    }

    public class ResponseBase : ResponseBase<object>
    {
        public ResponseBase() : base(null)
        {
        }

        public ResponseBase(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }

    public enum ReturnType
    {
        None,
        Ok,
        NotFound,
        Error
    }
}
