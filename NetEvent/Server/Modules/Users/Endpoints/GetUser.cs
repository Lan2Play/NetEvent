using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class GetUser
    {
        public static Task<IResult> Handle(ApplicationDbContext userDbContext, string id)
        {
            var user = userDbContext.Users.Where(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (user == null)
            {
                return Task.FromResult(Results.NotFound());
            }

            return Task.FromResult(Results.Ok(user));
        }
    }
}
