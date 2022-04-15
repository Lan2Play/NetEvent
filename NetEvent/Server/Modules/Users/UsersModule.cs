﻿using NetEvent.Server.Modules.Users.Endpoints;

namespace NetEvent.Server.Modules.Users
{
    public class UsersModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/users", GetUsers.Handle);
            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection builder)
        {
            return builder;
        }
    }
}