using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules.Themes.Endpoints.GetTheme;
using NetEvent.Server.Modules.Themes.Endpoints.PutTheme;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Themes
{
    public class ThemesModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/themes/theme", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetThemeRequest())));
            endpoints.MapPut("/api/themes/theme", async ([FromBody] ThemeDto theme, [FromServices] IMediator m) => ToApiResult(await m.Send(new PutThemeRequest(theme))));
            return endpoints;
        }

        public override IServiceCollection RegisterModule(IServiceCollection builder)
        {
            return builder;
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
