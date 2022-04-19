using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules.Users.Endpoints;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users
{
    public class ThemesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/theme", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetThemeRequest())));
            endpoints.MapPut("/theme", async ([FromBody] Theme theme, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutThemeRequest(theme))));
            return endpoints;
        }

        public override IServiceCollection RegisterModule(IServiceCollection builder)
        {
            return builder;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Theme>(entity =>
            {
                entity.ToTable(name: "Themes");
            });
        }
    }
}
