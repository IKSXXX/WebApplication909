using System.Security.Claims;
using System.Security.Principal;

namespace WebApplication909.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this IPrincipal user)
        {
            if (user is ClaimsPrincipal claimsPrincipal)
                return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            return null;
        }
    }
}