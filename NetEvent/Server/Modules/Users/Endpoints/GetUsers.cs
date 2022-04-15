using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetUsers
    {
        public static async Task<IResult> Handle(ApplicationDbContext userDbContext)
        {
            var allUsers = userDbContext.Users.ToList();
            return Results.Ok(allUsers);
        }
    }
}
