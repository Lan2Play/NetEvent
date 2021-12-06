using NetEvent.Server.Data;
using NetEvent.Server.Models;

namespace NetEvent.Server.GraphQl
{
    public class Query
    {
        //[UseApplicationDbContext]
        [Serial]
        public IQueryable<ApplicationUser> GetUsers(/*[ScopedService]*/[Service] ApplicationDbContext dbContext) => dbContext.Users;

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

            public Author Author { get; set; }
        }

        public class Author
        {
            public string Name { get; set; }
        }
    }


}
