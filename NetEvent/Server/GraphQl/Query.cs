using HotChocolate.AspNetCore.Authorization;
using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Query
    {
        [UseApplicationDbContext]
        public IQueryable<ApplicationUser> GetUsers([ScopedService] ApplicationDbContext dbContext) => dbContext.Users;

        public Book GetBook() =>
                new Book
                {
                    Title = "C# in depth.",
                    Author = new Author
                    {
                        Name = "Jon Skeet"
                    }
                };

        public class Book
        {
            public string Title { get; set; }

            [Authorize(Roles = new[] { "Admin" })]
            public Author Author { get; set; }
        }

        public class Author
        {
            public string Name { get; set; }
        }
    }
}
