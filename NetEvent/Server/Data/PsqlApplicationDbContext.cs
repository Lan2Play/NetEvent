using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetEvent.Server.Modules;

namespace NetEvent.Server.Data
{
    public class PsqlApplicationDbContext : ApplicationDbContext
    {
        private readonly IConfiguration _Configuration;

        public PsqlApplicationDbContext(DbContextOptions optionsBuilder, IConfiguration configuration, IReadOnlyCollection<IModule> modules)
            : base(optionsBuilder, modules)
        {
            _Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(_Configuration["DBConnection"]);
    }
}
