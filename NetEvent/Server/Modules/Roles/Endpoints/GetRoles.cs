using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetEvent.Server.Data;

namespace NetEvent.Server.Modules.Roles.Endpoints
{
    public class GetRoles
    {
        public static Task<IResult> Handle(ApplicationDbContext userDbContext)
        {
            var allRoles = userDbContext.Roles.ToList();
            return Task.FromResult(Results.Ok(allRoles));
        }
    }
}
