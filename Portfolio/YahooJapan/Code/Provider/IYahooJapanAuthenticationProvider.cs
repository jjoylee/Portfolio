using System.Threading.Tasks;

namespace Lib.Owin.Security.YahooJapan.Provider
{
    public interface IYahooJapanAuthenticationProvider
    {
        Task Authenticated(YahooJapanAuthenticatedContext context);
        void ApplyRedirect(YahooJapanApplyRedirectContext context);
        Task ReturnEndpoint(YahooJapanReturnEndpointContext context);
    }
}
