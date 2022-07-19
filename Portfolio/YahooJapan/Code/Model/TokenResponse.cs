using Newtonsoft.Json.Linq;

namespace Lib.Owin.Security.YahooJapan.Model
{
    public class TokenResponse
    {
        public TokenResponse(JObject response)
        {
            AccessToken = response["access_token"].Value<string>();
            RefreshToken = response["refresh_token"].Value<string>();
            ExpiresIn = response["expires_in"].Value<string>();
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
        public string ExpiresIn { get; }
    }
}
