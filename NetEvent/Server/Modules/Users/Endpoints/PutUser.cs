using Microsoft.AspNetCore.Mvc;
using NetEvent.Server.Data;
using NetEvent.Shared.Models;

namespace NetEvent.Server.Modules.Users.Endpoints
{
    public class PutRole
    {
        public static async Task<IResult> Handle(ApplicationDbContext userDbContext, string id, [FromBody] ApplicationUser user)
        {
            var oldUser = userDbContext.Find<ApplicationUser>(id);

            if (oldUser == null)
            {
                return Results.NotFound();
            }

            UpdateOldUser(oldUser, user);
            userDbContext.Update(oldUser);
            await userDbContext.SaveChangesAsync();
            return Results.Ok();
        }

        private static void UpdateOldUser(ApplicationUser oldUser, ApplicationUser user)
        {
            oldUser.EmailConfirmed = user.EmailConfirmed;
            oldUser.UserName = user.UserName;
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
        }
    }
}
