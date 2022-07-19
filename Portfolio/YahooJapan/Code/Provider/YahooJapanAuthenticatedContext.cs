using System;
using System.Globalization;
using System.Security.Claims;
using Lib.Owin.Security.YahooJapan.Model;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace Lib.Owin.Security.YahooJapan
{
    public class YahooJapanAuthenticatedContext : BaseContext
    {
        public YahooJapanAuthenticatedContext(IOwinContext context, TokenResponse tokenResponse, UserInfoResponse userInfoResponse)
            : base(context)
        {
            User = userInfoResponse.Response;
            AccessToken = tokenResponse.AccessToken;
            RefreshToken = tokenResponse.RefreshToken;
            int expiresValue;

            if (Int32.TryParse(tokenResponse.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue))
            {
                ExpiresIn = TimeSpan.FromSeconds(expiresValue);
            }
            Id = userInfoResponse.Id;
            Email = userInfoResponse.Email;
        }

        /// <summary>
        /// Gets the JSON-serialized user
        /// </summary>
        public JObject User { get; private set; }

        /// <summary>
        /// Gets the Yahoo access token
        /// </summary>
        public string AccessToken { get; private set; }
        /// <summary>
        /// Gets the Yahoo refresh token
        /// </summary>
        public string RefreshToken { get; private set; }

        /// <summary>
        /// Gets the Yahoo access token expiration time
        /// </summary>
        public TimeSpan? ExpiresIn { get; private set; }

        public ClaimsIdentity Identity { get; set; }
        public string Id { get; private set; }
        public string Email { get; private set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }

    }
}
