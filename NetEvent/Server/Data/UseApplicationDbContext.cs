using System.Runtime.CompilerServices;

namespace NetEvent.Server.Data
{
    public class UseApplicationDbContext : UseDbContextAttribute
    {
        public UseApplicationDbContext([CallerLineNumber] int order = 0) : base(typeof(ApplicationDbContext))
        {
            Order = order;
        }
    }
}
