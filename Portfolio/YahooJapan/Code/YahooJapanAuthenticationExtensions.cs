using Lib.Owin.Security.YahooJapan;
using System;

namespace Owin
{
    /// <summary>
    /// Extension methods for using <see cref="YahooJapanAuthenticationMiddleware"/>
    /// </summary>
    public static class YahooJapanAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate users using Yahoo
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseYahooJapanAuthentication(this IAppBuilder app, YahooJapanAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(YahooJapanAuthenticationMiddleware), app, options);
            return app;
        }

        /// <summary>
        /// Authenticate users using Yahoo
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="clientId">The Yahoo-issued client id</param>
        /// <param name="clientSecret">The Yahoo-issued client secret</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseYahooJapanAuthentication(
            this IAppBuilder app,
            string clientId,
            string clientSecret)
        {
            return UseYahooJapanAuthentication(
                app,
                new YahooJapanAuthenticationOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                });
        }
    }
}
