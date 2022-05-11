using System.Linq;
using System.Security.Claims;

namespace NetEvent.Server.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
        }
    }
}
