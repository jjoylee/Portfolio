using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace JangBoGo.Owin.Security.YahooJapan.Provider
{
    public class YahooJapanReturnEndpointContext : ReturnEndpointContext
    {
        /// <param name="context">OWIN environment</param>
        /// <param name="ticket">The authentication ticket</param>
        public YahooJapanReturnEndpointContext(IOwinContext context, AuthenticationTicket ticket)
         : base(context, ticket)
        {
        }
    }
}
