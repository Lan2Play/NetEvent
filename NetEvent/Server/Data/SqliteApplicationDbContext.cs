using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Modules;

namespace NetEvent.Server.Data
{

    public class SqliteApplicationDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _Configuration;

        public SqliteApplicationDbContext(DbContextOptions optionsBuilder, IConfiguration configuration, IReadOnlyCollection<IModule> modules)
            : base(optionsBuilder, modules)
        {
            _Configuration = configuration;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite( _Configuration["DBConnection"]);

    }

}
