using JangBoGo.Owin.Security.YahooJapan.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.Globalization;
using System.Net.Http;

namespace JangBoGo.Owin.Security.YahooJapan
{
    /// <summary>
    /// OWIN middleware for authenticating users using Yahoo
    /// </summary>
    public class YahooJapanAuthenticationMiddleware : AuthenticationMiddleware<YahooJapanAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a <see cref="YahooJapanAuthenticationMiddleware"/>
        /// </summary>
        /// <param name="next">The next middleware in the OWIN pipeline to invoke</param>
        /// <param name="app">The OWIN application</param>
        /// <param name="options">Configuration options for the middleware</param>
        public YahooJapanAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            YahooJapanAuthenticationOptions options)
            : base(next, options)
        {
            _logger = app.CreateLogger<YahooJapanAuthenticationMiddleware>();

            if (string.IsNullOrWhiteSpace(Options.ClientId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The '{0}' option must be provided.", "ClientId"));
            }

            if (string.IsNullOrWhiteSpace(Options.ClientSecret))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The '{0}' option must be provided.", "ClientSecret"));
            }

            if (Options.Provider == null)
            {
                Options.Provider = new YahooJapanAuthenticationProvider();
            }

            if (Options.StateDataFormat == null)
            {
                var dataProtector = app.CreateDataProtector(typeof(YahooJapanAuthenticationMiddleware).FullName, Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }

            _httpClient = new HttpClient(ResolveHttpMessageHandler(Options))
            {
                Timeout = Options.BackchannelTimeout,
                MaxResponseContentBufferSize = 1024 * 1024 * 10
            };
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="YahooJapanAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<YahooJapanAuthenticationOptions> CreateHandler()
        {
            return new YahooJapanAuthenticationHandler(_httpClient, _logger);
        }

        private HttpMessageHandler ResolveHttpMessageHandler(YahooJapanAuthenticationOptions options)
        {
            var handler = options.BackchannelHttpHandler ?? new WebRequestHandler();
            if (options.BackchannelCertificateValidator != null)
            {
                var webRequestHandler = handler as WebRequestHandler;
                if (webRequestHandler == null)
                {
                    throw new InvalidOperationException("An ICertificateValidator cannot be specified at the same time as an HttpMessageHandler unless it is a WebRequestHandler.");
                }
                webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;
            }
            return handler;
        }
    }
}
