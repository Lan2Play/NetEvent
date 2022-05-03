using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;

namespace NetEvent.Server.Data
{
    public static class AdminInitializer
    {
        public static void SeedData(NetEventUserManager userManager)
        {
            SeedUsers(userManager);
        }

        private static void SeedUsers(NetEventUserManager userManager)
        {
            if (userManager.FindByEmailAsync("admin@admin.de").Result == null)
            {
                ApplicationUser user = new ();
                user.UserName = "admin";
                user.Email = "admin@admin.de";
                user.FirstName = "Admin";
                user.EmailConfirmed = true;
                user.LastName = "istrator";

                IdentityResult result = userManager.CreateAsync(user, "Test123..").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
