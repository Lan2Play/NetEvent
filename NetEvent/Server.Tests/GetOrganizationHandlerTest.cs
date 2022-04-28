using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Organization.Endpoints.GetOrganization;
using Xunit;

namespace NetEvent.Server.Tests;

[ExcludeFromCodeCoverage]
public class GetOrganizationHandlerTest
{
    [Fact]
    public async Task GetOrganizationHandler_Success_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetOrganizationHandler_Success_Test))
            .Options;

        using var context = new ApplicationDbContext(options);
        var getOrganizationHandler = new GetOrganizationHandler(context);

        // Load Empty Data
        var getOrganizationRequest = new GetOrganizationRequest();
        var response = await getOrganizationHandler.Handle(getOrganizationRequest, CancellationToken.None).ConfigureAwait(false);

        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.NotNull(response.ReturnValue);
        Assert.Equal(0, response.ReturnValue?.Count);


        var testData = new[]{
            new OrganizationData{ Key = "key", Value = "value" },
            new OrganizationData{ Key = "key2", Value = "value2" }
        };

        await context.OrganizationData.AddRangeAsync(testData);
        await context.SaveChangesAsync();

        response = await getOrganizationHandler.Handle(getOrganizationRequest, CancellationToken.None).ConfigureAwait(false);

        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.NotNull(response.ReturnValue);
        Assert.Equal(2, response.ReturnValue?.Count);
        Assert.Equal(testData[0].Key, response.ReturnValue?[0].Key);
        Assert.Equal(testData[0].Value, response.ReturnValue?[0].Value);
        Assert.Equal(testData[1].Key, response.ReturnValue?[1].Key);
        Assert.Equal(testData[1].Value, response.ReturnValue?[1].Value);
    }
}
