using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Organization.Endpoints.PostOrganization;
using Xunit;

namespace NetEvent.Server.Tests;

[ExcludeFromCodeCoverage]
public class PostOrganizationHandlerTest
{
    [Fact]
    public async Task PostOrganizationHandler_Success_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(PostOrganizationHandler_Success_Test))
            .Options;

        using var context = new ApplicationDbContext(options);
        var postOrganizationHandler = new PostOrganizationHandler(context);

        // Add new Value
        var postOrganizationRequest = new PostOrganizationRequest(new Shared.Dto.OrganizationDataDto("key", "value"));
        var response = await postOrganizationHandler.Handle(postOrganizationRequest, CancellationToken.None).ConfigureAwait(false);
        var databaseOrganizationData = await context.FindAsync<OrganizationData>(postOrganizationRequest.OrganizationData.Key).ConfigureAwait(false);

        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.Equal("key", databaseOrganizationData?.Key);
        Assert.Equal("value", databaseOrganizationData?.Value);

        // Update value
        postOrganizationRequest = new PostOrganizationRequest(new Shared.Dto.OrganizationDataDto("key", "value2"));
        response = await postOrganizationHandler.Handle(postOrganizationRequest, CancellationToken.None).ConfigureAwait(false);
        databaseOrganizationData = await context.FindAsync<OrganizationData>(postOrganizationRequest.OrganizationData.Key).ConfigureAwait(false);

        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.Equal("key", databaseOrganizationData?.Key);
        Assert.Equal("value2", databaseOrganizationData?.Value);
    }

    [Fact]
    public async Task PostOrganizationHandler_Error_Test()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(PostOrganizationHandler_Success_Test))
            .Options;

        using var context = new ApplicationDbContext(options);
        var postOrganizationHandler = new PostOrganizationHandler(context);

        // Add new Value
        var postOrganizationRequest = new PostOrganizationRequest(new Shared.Dto.OrganizationDataDto(null, "value"));
        var response = await postOrganizationHandler.Handle(postOrganizationRequest, CancellationToken.None).ConfigureAwait(false);
        
        Assert.Equal(Modules.ReturnType.Error, response.ReturnType);

        // Update value
        postOrganizationRequest = new PostOrganizationRequest(new Shared.Dto.OrganizationDataDto("key", null));
        response = await postOrganizationHandler.Handle(postOrganizationRequest, CancellationToken.None).ConfigureAwait(false);
        
        Assert.Equal(Modules.ReturnType.Error, response.ReturnType);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
