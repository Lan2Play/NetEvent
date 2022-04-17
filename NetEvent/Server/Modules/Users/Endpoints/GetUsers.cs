using NetEvent.Server.Data;
using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetUsers
    {
        public static Task<IResult> Handle(ApplicationDbContext userDbContext)
        {
            var allUsers = userDbContext.Users.ToList();
            return Task.FromResult(Results.Ok(allUsers));
        }
    }
}
