using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Tests
{
    [ExcludeFromCodeCoverage]
    internal static class Fakers
    {
        public static Faker<ApplicationUser> ApplicationUserFaker() => new Faker<ApplicationUser>()
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.PasswordHash, (f, u) => f.Internet.Password())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
            .RuleFor(u => u.UserName, (f, u) => u.Email)
            .RuleFor(u => u.Id, (f, u) => Guid.NewGuid().ToString());

        public static Faker<UserDto> UserFaker() => new Faker<UserDto>()
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
            .RuleFor(u => u.UserName, (f, u) => u.Email)
            .RuleFor(u => u.ProfileImage, (f, u) => f.Random.Bytes(10))
            .RuleFor(u => u.Id, (f, u) => Guid.NewGuid().ToString());

        public static Faker<IdentityRole> IdentityRoleFaker() => new Faker<IdentityRole>()
              .RuleFor(r => r.Name, (f, r) => f.Name.JobArea())
              .RuleFor(r => r.NormalizedName, (f, r) => r.Name.ToUpperInvariant())
              .RuleFor(r => r.Id, (f, u) => Guid.NewGuid().ToString());

        public static Faker<RoleDto> RoleFaker(int numOfClaims = -1) => new Faker<RoleDto>()
              .RuleFor(r => r.Name, (f, r) => f.Name.JobArea())
              .RuleFor(r => r.Id, (f, r) => Guid.NewGuid().ToString())
              .RuleFor(r => r.Claims, (f, r) => ClaimFaker().Generate(numOfClaims < 0 ? Random.Shared.Next(100) : numOfClaims).Select(x => x.Type));

        public static Faker<ClaimDto> ClaimFaker() => new Faker<ClaimDto>()
              .RuleFor(r => r.Type, (f, r) => f.Name.JobType())
              .RuleFor(r => r.Value, (f, u) => string.Empty);
    }
}
