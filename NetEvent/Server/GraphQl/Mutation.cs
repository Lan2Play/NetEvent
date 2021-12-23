using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Mutation
    {
        [UseApplicationDbContext]
        public Task<UpdatePayload> UpdateUser([ScopedService] ApplicationDbContext dbContext, ApplicationUser user)
        {
            dbContext.Users.Update(user);
            return Task.FromResult(new UpdatePayload { Success = true });
        }
    }
}
