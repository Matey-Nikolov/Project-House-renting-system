using System.Security.Claims;

namespace HouseRentingSystem.Infrastructure
{
    public class ClaimsPrincipalExtensions
    {
        public static string Id(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}