using System.Web.Configuration;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;

namespace Google.Apis.Sample.MVC4
{
    public class AppFlowMetadata : FlowMetadata
    {
        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                        {
                            ClientId = WebConfigurationManager.AppSettings["ClientId"] ?? "notfound",
                            ClientSecret = WebConfigurationManager.AppSettings["ClientSecret"] ?? "notfound"
                        },
                    Scopes = new[] { "https://spreadsheets.google.com/feeds", "https://docs.google.com/feeds" },
                    
                });

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.

            return "kirkegata37";
        }

        public string Redirect {
            get { return "urn:ietf:wg:oauth:2.0:oob"; }
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}