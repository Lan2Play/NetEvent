using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public class PutRole
    {
        public static async Task<IResult> Handle(ApplicationDbContext userDbContext, string id, [FromBody] IdentityRole role)
        {
            var oldRole = userDbContext.Find<IdentityRole>(id);

            if (oldRole == null)
            {
                return Results.NotFound();
            }

            UpdateOldRole(oldRole, role);
            userDbContext.Update(oldRole);
            await userDbContext.SaveChangesAsync();
            return Results.Ok();
        }

        private static void UpdateOldRole(IdentityRole oldRole, IdentityRole user)
        {
            oldRole.EmailConfirmed = user.EmailConfirmed;
            oldRole.UserName = user.UserName;
            oldRole.FirstName = user.FirstName;
            oldRole.LastName = user.LastName;
        }
    }
}
