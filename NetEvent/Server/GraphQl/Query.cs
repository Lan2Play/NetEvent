using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Query
    {
        [UseApplicationDbContext]
        public IQueryable<ApplicationUser> GetUsers([ScopedService] ApplicationDbContext dbContext) => dbContext.Users;

        [UseApplicationDbContext]
        public IQueryable<IdentityRole> GetRoles([ScopedService] ApplicationDbContext dbContext) => dbContext.Roles;

        [UseApplicationDbContext]
        public IQueryable<IdentityUserRole<string>> GetUserRoles([ScopedService] ApplicationDbContext dbContext) => dbContext.UserRoles;
    }
}
