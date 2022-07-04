﻿using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.System.Endpoints.GetSystemInfo;
using NetEvent.Server.Modules.System.Endpoints.GetSystemSettings;
using NetEvent.Server.Modules.System.Endpoints.PostOrganization;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Modules.System
{
    [ExcludeFromCodeCoverage]
    public class SystemModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/system/info/all", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemInfoRequest())));
            endpoints.MapGet("/api/system/settings/{systemSettingGroup}/all", async ([FromRoute] SystemSettingGroup systemSettingGroup, [FromServices] IMediator m) => ToApiResult(await m.Send(new GetSystemSettingsRequest(systemSettingGroup))));
            endpoints.MapPost("/api/system/settings/{systemSettingGroup}", async ([FromRoute] SystemSettingGroup systemSettingGroup, [FromBody] Shared.Dto.SystemSettingValueDto systemSettingsValue, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostSystemSettingsRequest(systemSettingGroup, systemSettingsValue))));

            return endpoints;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SystemSettingValue>(entity =>
            {
                entity.ToTable(name: "OrganizationData");
                foreach (var setting in SystemSettings.Instance.OrganizationData)
                {
                    entity.HasData(new SystemSettingValue { Key = setting.Key, SerializedValue = setting.ValueType.DefaultValueSerialized });
                }
            });
        }
    }
}