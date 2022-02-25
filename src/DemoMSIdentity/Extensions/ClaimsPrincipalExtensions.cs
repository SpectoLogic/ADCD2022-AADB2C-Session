using System.Security.Claims;

namespace DemoMSIdentity.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal principal) => 
            principal.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.Email)?.Value ?? "unknown";
        public static string GetLoyaltiyID(this ClaimsPrincipal principal) =>
            principal?.Claims?.FirstOrDefault(c => c.Type == "extension_LoyaltiyID")?.Value ?? "unknown";
        public static string GetADCD_ID(this ClaimsPrincipal principal) =>
            principal?.Claims?.FirstOrDefault(c => c.Type == "extension_ADCD_ID")?.Value ?? "unknown";

    }
}
