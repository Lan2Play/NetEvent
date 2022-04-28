using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Organization.Endpoints.PostOrganization;
using Xunit;

namespace NetEvent.Server.Tests;

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
}
