using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetEvent.Server.Modules
{
    public abstract class ModuleBase : IModule
    {
        public abstract IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);

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
