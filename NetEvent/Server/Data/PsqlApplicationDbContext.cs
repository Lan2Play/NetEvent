using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetEvent.Server.Data
{

    public class PsqlApplicationDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _Configuration;

        public PsqlApplicationDbContext(DbContextOptions optionsBuilder, IConfiguration configuration)
            : base(optionsBuilder)
        {
            _Configuration = configuration;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql( _Configuration["DBConnection"]);

    }
}
