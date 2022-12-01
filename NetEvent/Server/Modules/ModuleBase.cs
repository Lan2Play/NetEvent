using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetEvent.Server.Modules
{
    public abstract class ModuleBase : IModule
    {
        public abstract IEndpointRouteBuilder MapModuleEndpoints(IEndpointRouteBuilder endpoints);

        public virtual IEndpointRouteBuilder MapModuleReadAuthEndpoints(IEndpointRouteBuilder endpoints) => endpoints;

        public virtual IEndpointRouteBuilder MapModuleWriteAuthEndpoints(IEndpointRouteBuilder endpoints) => endpoints;

        public virtual IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;

        public virtual IServiceCollection RegisterModule(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(ModuleBase));
            return builder;
        }

        public virtual void OnModelCreating(ModelBuilder builder)
        {
            // Do nothing in base
        }

        protected IResult ToApiResult<T>(ResponseBase<T> response)
        {
            if (response.ReturnValue is IResult result)
            {
                return result;
            }

            return response.ReturnType switch
            {
                ReturnType.Ok => Results.Ok(response.ReturnValue),
                ReturnType.NotFound => Results.NotFound(),
                ReturnType.Error => Results.BadRequest(response.Error),
                _ => throw new($"ReturnType {response.ReturnType} is not supported!"),
            };
        }
    }
}
