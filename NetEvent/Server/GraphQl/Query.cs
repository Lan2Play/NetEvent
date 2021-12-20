using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Query
    {
        [UseApplicationDbContext]
        [UseFiltering]
        public IQueryable<ApplicationUser> GetUsers([ScopedService] ApplicationDbContext dbContext) => dbContext.Users;

        [UseApplicationDbContext]
        public ApplicationUser? GetUser([ScopedService] ApplicationDbContext dbContext, string id) => dbContext.Users.Find(id);

        [UseApplicationDbContext]
        [UseFiltering]
        public IQueryable<IdentityRole> GetRoles([ScopedService] ApplicationDbContext dbContext) => dbContext.Roles;

        [UseApplicationDbContext]
        [UseFiltering]
        public IQueryable<IdentityUserRole<string>> GetUserRoles([ScopedService] ApplicationDbContext dbContext) => dbContext.UserRoles;
    }
}
