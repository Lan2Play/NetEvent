using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Server.Modules.Users.Endpoints.PutUser;
using Xunit;

namespace NetEvent.Server.Tests;

public class PutUserHandlerTest
{
    [Fact]
    public async Task PutUserHandler_Success_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "NetEvent")
            .Options;

        var applicationUserFaker = Fakers.ApplicationUserFaker();
        var userFaker = Fakers.UserFaker();

        using var contextSeed = new ApplicationDbContext(options);

        var applicationUser = applicationUserFaker.Generate();

        await contextSeed.Users.AddAsync(applicationUser).ConfigureAwait(false);

        contextSeed.SaveChanges();

        var user = userFaker.Generate();

        var logMock = new Mock<ILogger<PutUserHandler>>();

        using var context = new ApplicationDbContext(options);

        var putUserHandler = new PutUserHandler(context, logMock.Object);

        var putUserRequest = new PutUserRequest(applicationUser.Id, user);

        //Act
        var response = await putUserHandler.Handle(putUserRequest, CancellationToken.None).ConfigureAwait(false);

        var databaseUser = await context.FindAsync<ApplicationUser>(putUserRequest.Id).ConfigureAwait(false);

        //Assert
        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.Equal(user.FirstName, databaseUser?.FirstName);
        Assert.Equal(user.LastName, databaseUser?.LastName);
        Assert.Equal(user.EmailConfirmed, databaseUser?.EmailConfirmed);
        Assert.Equal(user.UserName, databaseUser?.UserName);
    }

    [Fact]
    public async Task PutUserHandler_NotFound_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "NetEvent")
            .Options;

        var logMock = new Mock<ILogger<PutUserHandler>>();

        var userFaker = Fakers.UserFaker();

        var user = userFaker.Generate();

        using var context = new ApplicationDbContext(options);

        var getUsersHandler = new PutUserHandler(context, logMock.Object);

        var getUsersRequest = new PutUserRequest(user.Id, user);

        //Act
        var response = await getUsersHandler.Handle(getUsersRequest, CancellationToken.None).ConfigureAwait(false);

        //Assert
        Assert.Equal(Modules.ReturnType.NotFound, response.ReturnType);
    }
}
