using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DemoMSIdentity.Authentication
{
    public class DemoClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            if (!principal.HasClaim(claim => claim.Type == ClaimTypes.Email))
            {
                var claimValue = principal.Claims.FirstOrDefault(c => c.Type == "emails")?.Value ?? "unknown";
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, claimValue));
            }

            principal.AddIdentity(claimsIdentity);
            return Task.FromResult(principal);
        }
    }
}
