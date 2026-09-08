using System.Security.Claims;

namespace dotnet_api_learning.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))?.Value;
            return user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
                ?? user.FindFirstValue(ClaimTypes.GivenName)
                ?? user.FindFirstValue("given_name")
                ?? user.FindFirstValue(ClaimTypes.Name);
        }
        
    }
}