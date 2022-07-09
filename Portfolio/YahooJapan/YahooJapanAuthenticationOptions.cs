using JangBoGo.Owin.Security.YahooJapan.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace JangBoGo.Owin.Security.YahooJapan
{
    public class YahooJapanAuthenticationOptions : AuthenticationOptions
    {
        public YahooJapanAuthenticationOptions() : base(Constants.DefaultAuthenticationType) {
            Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-yahoo");
            AuthenticationMode = AuthenticationMode.Passive;
            Scope = new List<string> { "openid", "profile", "email" };
            BackchannelTimeout = TimeSpan.FromSeconds(60);

            AuthorizationEndpoint = Constants.AuthorizationEndpoint;
            TokenEndpoint = Constants.TokenEndpoint;
            UserInformationEndpoint = Constants.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the Yahoo-assigned clientId
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Yahoo-assigned client secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Get or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        /// <summary>
        /// The request path within the application's base path where the user-agent will be returned.
        /// The middleware will process this request when it arrives.
        /// Default value is "/signin-yahoo".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// A list of permissions to request.
        /// </summary>
        public IList<string> Scope { get; set; }

        /// <summary>
        /// Gets or sets the URI where the client will be redirected to authenticate.
        /// The default value is 'https://auth.login.yahoo.co.jp/yconnect/v2/authorization'.
        /// </summary>
        public string AuthorizationEndpoint { get; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to exchange the OAuth token.
        /// The default value is 'https://auth.login.yahoo.co.jp/yconnect/v2/token'.
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the user information.
        /// The default value is 'https://userinfo.yahooapis.jp/yconnect/v2/attribute'.
        /// </summary>
        public string UserInformationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user <see cref="System.Security.Claims.ClaimsIdentity"/>.
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IYahooJapanAuthenticationProvider"/> used to handle authentication events.
        /// </summary>
        public IYahooJapanAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
        public TimeSpan BackchannelTimeout { get; set; }
        public HttpMessageHandler BackchannelHttpHandler { get; set; }
        public ICertificateValidator BackchannelCertificateValidator { get; set; }
    }
}
