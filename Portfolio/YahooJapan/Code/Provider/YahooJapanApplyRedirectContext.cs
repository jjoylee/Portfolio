using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace Lib.Owin.Security.YahooJapan.Provider
{
    public class YahooJapanApplyRedirectContext : BaseContext<YahooJapanAuthenticationOptions>
    {
        public YahooJapanApplyRedirectContext(IOwinContext context, YahooJapanAuthenticationOptions options, AuthenticationProperties properties, string redirectUri)
            :base(context, options)
        {
            RedirectUri = redirectUri;
            Properties = properties;
        }

        /// <summary>
        /// Gets the URI used for the redirect operation.
        /// </summary>
        public string RedirectUri { get; private set; }

        /// <summary>
        /// Gets the authentication properties of the challenge
        /// </summary>
        public AuthenticationProperties Properties { get; private set; }
    }
}
