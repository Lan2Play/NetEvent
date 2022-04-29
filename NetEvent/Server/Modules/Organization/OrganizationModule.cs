﻿using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Organization.Endpoints.GetOrganization;
using NetEvent.Server.Modules.Organization.Endpoints.PostOrganization;

namespace NetEvent.Server.Modules.Authorization
{
    [ExcludeFromCodeCoverage]
    public class OrganizationModule : ModuleBase
    {
        public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/organization/all", async ([FromServices] IMediator m) => ToApiResult(await m.Send(new GetOrganizationRequest())));
            endpoints.MapPost("/api/organization", async ([FromBody] Shared.Dto.OrganizationDataDto organizationData, [FromServices] IMediator m) => ToApiResult(await m.Send(new PostOrganizationRequest(organizationData))));

            return endpoints;
        }

        public override IServiceCollection RegisterModule(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(OrganizationModule));
            return builder;
        }

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrganizationData>(entity =>
            {
                entity.ToTable(name: "OrganizationData");
            });
        }
    }
}