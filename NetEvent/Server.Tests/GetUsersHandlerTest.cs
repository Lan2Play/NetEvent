using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NetEvent.Server.Data;
using NetEvent.Server.Modules.Users.Endpoints.GetUsers;
using Xunit;

namespace NetEvent.Server.Tests;

[ExcludeFromCodeCoverage]
public class GetUsersHandlerTest
{
    [Fact]
    public async Task GetUsersHandler_Success_Test()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetUsersHandler_Success_Test))
            .Options;

        var userFaker = Fakers.ApplicationUserFaker();

        var usersCount = 5;

        using var contextSeed = new ApplicationDbContext(options);

        var users = userFaker.Generate(usersCount);

        await contextSeed.Users.AddRangeAsync(users).ConfigureAwait(false);

        contextSeed.SaveChanges();

        var logMock = new Mock<ILogger<GetUsersHandler>>();

        using var context = new ApplicationDbContext(options);

        var getUsersHandler = new GetUsersHandler(context, logMock.Object);

        var getUsersRequest = new GetUsersRequest();

        //Act
        var response = await getUsersHandler.Handle(getUsersRequest, CancellationToken.None).ConfigureAwait(false);

        //Assert
        Assert.Equal(Modules.ReturnType.Ok, response.ReturnType);
        Assert.NotNull(response.ReturnValue);
        Assert.Equal(usersCount, response.ReturnValue?.Count());
    }
}
