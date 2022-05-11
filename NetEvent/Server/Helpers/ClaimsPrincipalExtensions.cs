using System.Linq;
using System.Security.Claims;

namespace NetEvent.Server.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
