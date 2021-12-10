using HotChocolate.AspNetCore.Authorization;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Query
    {
        [UseApplicationDbContext]
        public IQueryable<ApplicationUser> GetUsers([ScopedService] ApplicationDbContext dbContext) => dbContext.Users;
    }
}
