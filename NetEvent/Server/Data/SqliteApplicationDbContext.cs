using Microsoft.EntityFrameworkCore;

namespace NetEvent.Server.Data
{

    public class SqliteApplicationDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _Configuration;

        public SqliteApplicationDbContext(DbContextOptions optionsBuilder, IConfiguration configuration)
            : base(optionsBuilder)
        {
            _Configuration = configuration;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite( _Configuration["DBConnection"]);

    }

}
