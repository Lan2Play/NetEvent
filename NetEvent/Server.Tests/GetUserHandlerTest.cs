using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NetEvent.Server.Data;
using NetEvent.Server.Modules.Users.Endpoints.GetUser;
using Xunit;

namespace NetEvent.Server.Tests;

[ExcludeFromCodeCoverage]
public class GetUserHandlerTest
{
    [Fact]
    public async Task GetUserHandler_Success_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetUserHandler_Success_Test))
            .Options;

        var userFaker = Fakers.ApplicationUserFaker();

        using var contextSeed = new ApplicationDbContext(options);

        var user = userFaker.Generate();

        await contextSeed.Users.AddAsync(user).ConfigureAwait(false);

        contextSeed.SaveChanges();

        var logMock = new Mock<ILogger<GetUserHandler>>();

        using var context = new ApplicationDbContext(options);

        var getUserHandler = new GetUserHandler(context, logMock.Object);

        var getUserRequest = new GetUserRequest(user.Id);

        //Act
        var response = await getUserHandler.Handle(getUserRequest, CancellationToken.None).ConfigureAwait(false);

        //Assert
        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.NotNull(response.ReturnValue);
        Assert.Equal(user.Id, response.ReturnValue?.Id);
        Assert.Equal(user.FirstName, response.ReturnValue?.FirstName);
        Assert.Equal(user.LastName, response.ReturnValue?.LastName);
    }

    [Fact]
    public async Task GetUserHandler_NotFound_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetUserHandler_NotFound_Test))
            .Options;

        var logMock = new Mock<ILogger<GetUserHandler>>();

        using var context = new ApplicationDbContext(options);

        var getUserHandler = new GetUserHandler(context, logMock.Object);

        var getUserRequest = new GetUserRequest("notexistingid");

        //Act
        var response = await getUserHandler.Handle(getUserRequest, CancellationToken.None).ConfigureAwait(false);

        //Assert
        Assert.Equal(Modules.ReturnType.NotFound, response.ReturnType);
    }
}
