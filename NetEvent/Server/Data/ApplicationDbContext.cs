using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetEvent.Server.Models;
using NetEvent.Server.Modules;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Policy;

namespace NetEvent.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private const string _AdminGuid = "BAFC89CF-4F3E-4595-8256-CCA19C260FBD";
        private readonly IReadOnlyCollection<IModule> _Modules;

        public ApplicationDbContext(DbContextOptions options, IReadOnlyCollection<IModule> modules)
            : base(options)
        {
            _Modules = modules;
        }

        public virtual DbSet<ThemeDto> Themes => Set<ThemeDto>();

        public virtual DbSet<SystemSettingValue> SystemSettingValues => Set<SystemSettingValue>();

        public virtual DbSet<SystemImage> SystemImages => Set<SystemImage>();

        public virtual DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();

        public virtual DbSet<Event> Events => Set<Event>();

        public virtual DbSet<Venue> Venues => Set<Venue>();

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
                _ = entity.HasData(adminUser);
            });
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Role");

                entity.HasData(new ApplicationRole { Id = "user", Name = "User", NormalizedName = "USER", IsDefault = true });
                entity.HasData(new ApplicationRole { Id = "orga", Name = "Orga", NormalizedName = "ORGA" });
                entity.HasData(new ApplicationRole { Id = "admin", Name = "Admin", NormalizedName = "ADMIN" });
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasData(new IdentityUserRole<string> { UserId = _AdminGuid, RoleId = "admin" });
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

                var policyCounter = 1;
                foreach (var policy in Policies.AvailablePolicies)
                {
                    entity.HasData(new IdentityRoleClaim<string> { Id = policyCounter, ClaimType = policy, RoleId = "admin", ClaimValue = string.Empty });
                    policyCounter++;
                }
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<EmailTemplate>(entity =>
            {
                entity.ToTable(name: nameof(EmailTemplates));

                entity.HasData(new EmailTemplate { TemplateId = "UserEmailConfirmEmailTemplate", ContentTemplate = "<h1>@Model.TemplateVariables[\"firstName\"], welcome to NetEvent.</h1>\n<p> Please confirm your E-Mail by clicking on the following link:</p><a href=\"@Model.TemplateVariables[\"confirmUrl\"]\">@Model.TemplateVariables[\"confirmUrl\"]</a>   ", SubjectTemplate = "@Model.TemplateVariables[\"firstName\"], please confirm your E-Mail address." });
            });

            if (_Modules != null)
            {
                foreach (var module in _Modules)
                {
                    module.OnModelCreating(builder);
                }
            }
        }
    }
}
