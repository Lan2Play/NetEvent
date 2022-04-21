using Microsoft.EntityFrameworkCore;

namespace NetEvent.Server.Data
{

    public class SqliteApplicationDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _Configuration;

        public SqliteApplicationDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            this._Configuration = configuration;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite( _Configuration["DBConnection"]);

    }

}