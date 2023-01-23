using System.Security.Claims;

namespace RunGroupWebApp
{
    public static class ClaimsPrincipalExtensions
    {
        // passing in "this" ClaimsPrincipal with name user
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
