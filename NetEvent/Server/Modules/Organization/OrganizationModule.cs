using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Organization.Endpoints.GetOrganization;
using NetEvent.Server.Modules.Organization.Endpoints.PostOrganization;
using NetEvent.Shared.Constants;

namespace NetEvent.Server.Modules.Organization
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

        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrganizationData>(entity =>
            {
                entity.ToTable(name: "OrganizationData");
                entity.HasData(new OrganizationData { Key = OrganizationDataConstants.CultureKey, Value = "en-US" });
            });
        }
    }
}
