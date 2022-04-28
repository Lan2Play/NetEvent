using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Modules;
using NetEvent.Shared.Dto;
using NetEvent.Server.Models;

namespace NetEvent.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private const string _AdminGuid = "BAFC89CF-4F3E-4595-8256-CCA19C260FBD";
        private const string _RoleAdminGuid = "FEAF344F-AA9B-47F5-B170-829617CDD9A4";
        private const string _RoleUserGuid = "3ECB400B-DFCF-4268-8D67-7BC9F09DD0B1";

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<ThemeDto> Themes { get; set; }

        public virtual DbSet<OrganizationData> OrganizationData { get; set; }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return base.Add(entity);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
                var adminUser = new ApplicationUser
                {
                    Id = _AdminGuid,
                    UserName = "admin",
                    NormalizedUserName = "admin".ToUpper(),
                    Email = "admin@admin.de",
                    NormalizedEmail = "admin@admin.de".ToUpper(),
                    FirstName = "Admin",
                    EmailConfirmed = true,
                    LastName = "istrator"
                };
                adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "Test123..");
                _ = entity.HasData(adminUser); ;
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");

                // Seed Roles by Enum
                entity.HasData(new IdentityRole { Id = "admin", Name = "Admin", NormalizedName = "ADMIN" });
                entity.HasData(new IdentityRole { Id = "user", Name = "User", NormalizedName = "USER" });
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasData(new IdentityUserRole<string> { UserId = _AdminGuid, RoleId = "admin"});
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.CreateModels();
        }
    }
}
