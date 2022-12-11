using System.Security.Claims;

namespace Bank.NET___backend
{
    public static class Helpers
    {
        public static Claim? GetClaim(List<Claim> claims, string claimType)
        {
            return claims.Find(x => x.Type == claimType);
        }
    }
}
