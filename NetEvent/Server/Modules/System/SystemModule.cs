using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.System.Endpoints;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Modules.System
{
    [ExcludeFromCodeCoverage]
    public class SystemModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/system/info/all", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemInfo.Request())));
            endpoints.MapGet("/api/system/settings/{systemSettingGroup}/all", async ([FromRoute] SystemSettingGroup systemSettingGroup, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemSettings.Request(systemSettingGroup))));
            endpoints.MapGet("/api/system/settings/{systemSettingGroup}/{settingKey}", async ([FromRoute] SystemSettingGroup systemSettingGroup, [FromRoute] string settingKey, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemSetting.Request(systemSettingGroup, settingKey))));
            endpoints.MapPost("/api/system/settings/{systemSettingGroup}", async ([FromRoute] SystemSettingGroup systemSettingGroup, [FromBody] Shared.Dto.SystemSettingValueDto systemSettingsValue, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostSystemSetting.Request(systemSettingGroup, systemSettingsValue))));
            endpoints.MapPost("/api/system/image/{imageName}", HandleImageUpload);
            endpoints.MapPost("/api/system/editorimage", HandleEditorImageUpload);
            endpoints.MapGet("/api/system/image/{imageName}", async ([FromRoute] string imageName, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemImage.Request(imageName))));
            endpoints.MapDelete("/api/system/image/{imageName}", async ([FromRoute] string imageName, [FromServices] IMediator m) => ToApiResult(await m.Send(new DeleteSystemImage.Request(imageName))));
            endpoints.MapGet("/api/system/image/all", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemImages.Request())));
            endpoints.MapGet("/favicon.ico", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemImage.Request(SystemSettings.Favicon))));
            endpoints.MapGet("/favicon.png", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemImage.Request(SystemSettings.Favicon))));

            return endpoints;
        }

        private async Task<IResult> HandleImageUpload(HttpRequest request, [FromRoute] string imageName, [FromServices] IMediator mediator)
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest();
            }

            var form = await request.ReadFormAsync();
            var formFile = form.Files[0];

            if (formFile is null || formFile.Length == 0)
            {
                return Results.BadRequest();
            }

            await using var stream = formFile.OpenReadStream();

            return ToApiResult(await mediator.Send(new PostSystemImage.Request(imageName, formFile)));
        }

        private async Task<IResult> HandleEditorImageUpload(HttpRequest request, [FromServices] IMediator mediator)
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest();
            }

            var form = await request.ReadFormAsync();
            var formFile = form.Files[0];

            if (formFile is null || formFile.Length == 0)
            {
                return Results.BadRequest();
            }

            await using var stream = formFile.OpenReadStream();
            var result = await mediator.Send(new PostSystemImage.Request(formFile.FileName, formFile));

            if (result.ReturnType != ReturnType.Ok)
            {
                return ToApiResult(result);
            }

            return Results.Json(new { location = $"/api/system/image/{result.ReturnValue}" });
            //return ToApiResult(await mediator.Send(new PostSystemImage.Request(imageName, formFile)));
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SystemSettingValue>(entity =>
            {
                entity.ToTable(name: "SystemSettings");
                foreach (var setting in SystemSettings.Instance.OrganizationData
                                .Concat(SystemSettings.Instance.AuthenticationData))
                {
                    entity.HasData(new SystemSettingValue { Key = setting.Key, SerializedValue = setting.ValueType.DefaultValueSerialized });
                }
            });

            builder.Entity<SystemImage>(entity =>
            {
                entity.ToTable(name: "SystemImages");
            });
        }
    }
}
