using System.Threading.Tasks;

namespace JangBoGo.Owin.Security.YahooJapan.Provider
{
    public interface IYahooJapanAuthenticationProvider
    {
        Task Authenticated(YahooJapanAuthenticatedContext context);
        void ApplyRedirect(YahooJapanApplyRedirectContext context);
        Task ReturnEndpoint(YahooJapanReturnEndpointContext context);
    }
}
