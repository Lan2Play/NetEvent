using Microsoft.EntityFrameworkCore;

namespace NetEvent.Server.Data
{

    public class PsqlApplicationDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _Configuration;

        public PsqlApplicationDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            this._Configuration = configuration;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql( _Configuration["DBConnection"]);

    }
}