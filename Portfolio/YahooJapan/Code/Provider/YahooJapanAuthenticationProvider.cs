using System;
using System.Threading.Tasks;

namespace Lib.Owin.Security.YahooJapan.Provider
{
    public class YahooJapanAuthenticationProvider : IYahooJapanAuthenticationProvider
    {
        public YahooJapanAuthenticationProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
            OnApplyRedirect = context => context.Response.Redirect(context.RedirectUri);
        }

        public Task Authenticated(YahooJapanAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }
        
        /// <summary>
        /// Gets or sets the function that is invoked when the Authenticated method is invoked.
        /// </summary>
        public Func<YahooJapanAuthenticatedContext, Task> OnAuthenticated { get; set; }

        public Task ReturnEndpoint(YahooJapanReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }

        /// <summary>
        /// Gets or sets the function that is invoked when the ReturnEndpoint method is invoked.
        /// </summary>
        public Func<YahooJapanReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

        public void ApplyRedirect(YahooJapanApplyRedirectContext context)
        {
            OnApplyRedirect(context);
        }

        /// <summary>
        /// Gets or sets the delegate that is invoked when the ApplyRedirect method is invoked.
        /// </summary>
        public Action<YahooJapanApplyRedirectContext> OnApplyRedirect { get; set; }
    }
}
