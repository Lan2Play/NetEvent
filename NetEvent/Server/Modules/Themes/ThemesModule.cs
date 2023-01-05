using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules.Themes.Endpoints;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes
{
    public class ThemesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapModuleEndpoints(IEndpointRouteBuilder endpoints)
        {
            // BaseRoute: /api/themes
            endpoints.MapGet("/theme", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetTheme.Request())));
            endpoints.MapPut("/theme", async ([FromBody] ThemeDto theme, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutTheme.Request(theme))));
            return endpoints;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ThemeDto>(entity =>
            {
                entity.ToTable(name: "Themes");
            });
        }
    }
}
