using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using Slugify;

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

        public static Faker<ApplicationRole> ApplicationRoleFaker() => new Faker<ApplicationRole>()
            .RuleFor(r => r.IsDefault, (f, r) => f.IndexVariable == 0)
            .RuleFor(r => r.Name, (f, r) => f.Internet.UserName(f.UniqueIndex.ToString()))
            .RuleFor(r => r.NormalizedName, (f, r) => r.Name?.ToUpperInvariant())
            .RuleFor(r => r.Id, (f, u) => Guid.NewGuid().ToString());

        public static Faker<RoleDto> RoleFaker(int numOfClaims = -1) => new Faker<RoleDto>()
              .RuleFor(r => r.Name, (f, r) => f.Name.JobArea())
              .RuleFor(r => r.Id, (f, r) => Guid.NewGuid().ToString())
              .RuleFor(r => r.Claims, (f, r) => ClaimFaker().Generate(numOfClaims < 0 ? Random.Shared.Next(100) : numOfClaims).Select(x => x.Type));

        public static Faker<ClaimDto> ClaimFaker() => new Faker<ClaimDto>()
              .RuleFor(r => r.Type, (f, r) => f.Name.JobType())
              .RuleFor(r => r.Value, (f, u) => string.Empty);

        public static Faker<Event> EventFaker(IEnumerable<Venue> venues) => new Faker<Event>()
         .RuleFor(v => v.Id, (f, v) => f.IndexFaker)
         .RuleFor(e => e.Name, (f, e) => f.Name.FirstName())
         .RuleFor(e => e.Slug, (f, e) => new SlugHelper().GenerateSlug(e.Name))
         .RuleFor(e => e.Description, (f, e) => f.Lorem.Sentences(10))
         .RuleFor(e => e.ShortDescription, (f, e) => f.Lorem.Sentence(10))
         .RuleFor(e => e.StartDate, (f, e) => DateTime.UtcNow.AddDays(Random.Shared.Next(1, 30)))
         .RuleFor(e => e.EndDate, (f, e) => e.StartDate?.AddDays(Random.Shared.Next(1, 30)))
         .RuleFor(e => e.Venue, (f, e) => venues.ElementAtOrDefault(Random.Shared.Next(venues.Count() - 1)))
         .RuleFor(e => e.VenueId, (f, e) => e.Venue?.Id ?? 0);

        public static Faker<Venue> VenueFaker() => new Faker<Venue>()
            .RuleFor(v => v.Id, (f, v) => f.IndexFaker)
            .RuleFor(v => v.Name, (f, v) => f.Name.FirstName())
            .RuleFor(v => v.Slug, (f, v) => new SlugHelper().GenerateSlug(v.Name))
            .RuleFor(v => v.Street, (f, v) => f.Address.StreetName())
            .RuleFor(v => v.Number, (f, v) => f.Address.BuildingNumber())
            .RuleFor(v => v.ZipCode, (f, v) => f.Address.ZipCode("#####"))
            .RuleFor(v => v.City, (f, v) => f.Address.City());

    }
}
