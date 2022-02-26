using DemoClaimsprovider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SpectoLogic.Identity.AADB2C.APIConnectors.Extensions;
using SpectoLogic.Identity.AADB2C.APIConnectors.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DemoClaimsprovider
{
    public class ClaimsProviderFunc
    {
        private const string BASIC_AUTH_USERNAME = "admina";
        private const string BASIC_AUTH_PASSWORD = "PleaseUseCertificatesInstead!";

        [FunctionName("EnrichClaims")]
        public async Task<IActionResult> EnrichClaims(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (!IsAuthenticated(req.Headers))
            {
                return new UnauthorizedResult();
            }

            try
            {
                var claimsReq = await req.Body.GetClaimsRequestAsync<MyClaimsRequest>();

                var myId = claimsReq.ADCD_ID;

                return new OkObjectResult(
                    new MyClaimsResponse()
                    {
                        ADCD_ID = $"Connector: {myId}",
                    })
                {
                    StatusCode = 200
                };
            }
            catch (Exception)
            {
                var msgResponse = new ErrorMessageResponse()
                {
                    Action = ResponseActions.ShowBlockPage,
                    UserMessage = "There was a problem with your request. You are not able to sign up at this time."
                };
                return new BadRequestObjectResult(msgResponse);
            }
        }

        /// <summary>
        /// HANDLE BASIC AUTHENTICATION
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private bool IsAuthenticated(IHeaderDictionary headers)
        {
            if (headers == null || !headers.ContainsKey("Authorization"))
            {
                return false;
            }

            string authHeader = headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return false;
            }

            var username = BASIC_AUTH_USERNAME;
            var password = BASIC_AUTH_PASSWORD;
            var encoded = Convert.ToBase64String(
                Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(username + ":" + password));

            var expectedHeader = $"Basic {encoded}";
            return expectedHeader == authHeader;
        }
    }
}
