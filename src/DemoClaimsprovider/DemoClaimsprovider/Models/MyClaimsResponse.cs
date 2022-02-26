using SpectoLogic.Identity.AADB2C.APIConnectors;

namespace DemoClaimsprovider.Models
{
    [ExtensionAppId("eff3e2b4-6308-437e-953f-95fec3dc1573")]
    [CustomClaim("ADCD_ID", typeof(string))]
    [CustomClaim("LoyaltiyID", typeof(string))]
    public partial class MyClaimsResponse
    {
    }
}
