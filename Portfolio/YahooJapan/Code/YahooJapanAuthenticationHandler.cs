using Lib.Owin.Security.YahooJapan.Model;
using Lib.Owin.Security.YahooJapan.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Owin.Security.YahooJapan
{
    public class YahooJapanAuthenticationHandler : AuthenticationHandler<YahooJapanAuthenticationOptions>
    {
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
        private HttpClient _httpClient;
        private ILogger _logger;

        public YahooJapanAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;
            try
            {
                var query = Request.Query;
                if (ValueExist(query, "error"))
                {
                    _logger.WriteVerbose($"Remote server returned an error: {Request.QueryString}");
                }

                var state = GetValue(query, "state");
                properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return null;
                }
                
                var code = GetValue(query, "code");
                if (!ValidateCorrelationId(properties, _logger) || code == null)
                {
                    return new AuthenticationTicket(null, properties);
                }

                var tokenResponse = await TokenEndpointRequest(code);
                var userInfoResponse = await UserInfoRequest(tokenResponse.AccessToken);

                var context = GetAuthenticatedContext(properties, tokenResponse, userInfoResponse);
                await Options.Provider.Authenticated(context);
                return new AuthenticationTicket(context.Identity, context.Properties);
            }
            catch (Exception ex)
            {
                _logger.WriteError("Authentication failed", ex);
                return new AuthenticationTicket(null, properties);
            }
        }
        
        private bool ValueExist(IReadableStringCollection query, string key)
        {
            var values = query.GetValues(key);
            return values != null && values.Count == 1;
        }

        private string GetValue(IReadableStringCollection query, string key)
        {
            return ValueExist(query, key) ? query.GetValues(key)[0] : null;
        }
        
        private async Task<TokenResponse> TokenEndpointRequest(string code)
        {
            var redirectUri = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Options.CallbackPath}";
            var contentPost = new StringContent(
                $@"grant_type=authorization_code
                   &client_id={Uri.EscapeDataString(Options.ClientId)}
                   &client_secret={Uri.EscapeDataString(Options.ClientSecret)}
                   &code={Uri.EscapeDataString(code)}
                   &redirect_uri={Uri.EscapeDataString(redirectUri)}"
                , Encoding.UTF8, "application/x-www-form-urlencoded"
            );

            var response = await _httpClient.PostAsync(Options.TokenEndpoint, contentPost);
            response.EnsureSuccessStatusCode();
            return new TokenResponse(JObject.Parse(await GetResponseString(response)));
        }
        
        private async Task<string> GetResponseString(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<UserInfoResponse> UserInfoRequest(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(Options.UserInformationEndpoint, Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            return new UserInfoResponse(JObject.Parse(await GetResponseString(response)));
        }

        private YahooJapanAuthenticatedContext GetAuthenticatedContext(AuthenticationProperties properties, TokenResponse tokenResponse, UserInfoResponse userInfoResponse)
        {
            var context = new YahooJapanAuthenticatedContext(Context, tokenResponse, userInfoResponse)
            {
                Identity = new ClaimsIdentity(Options.AuthenticationType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType)
            };

            if (!string.IsNullOrEmpty(context.Id))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.Id, XmlSchemaString, Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Email))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.Email, context.Email, XmlSchemaString, Options.AuthenticationType));
            }
            context.Properties = properties;
            return context;
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);
            if (challenge == null)
            {
                return Task.FromResult<object>(null);
            }

            var baseUri = $"{Request.Scheme}{Uri.SchemeDelimiter}{Request.Host}{Request.PathBase}";

            var properties = challenge.Properties;
            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = $"{baseUri}{Request.Path}{Request.QueryString}";
            }

            GenerateCorrelationId(properties);
            
            var authorizationEndpoint =
                Options.AuthorizationEndpoint +
                $"?response_type=code" +
                $"&redirect_uri={Uri.EscapeDataString($"{baseUri}{Options.CallbackPath}")}" +
                $"&client_id={Uri.EscapeDataString(Options.ClientId)}" +
                $"&scope={Uri.EscapeDataString(Options.Scope.StringJoin(" "))}" +
                $"&state={Uri.EscapeDataString(Options.StateDataFormat.Protect(properties))}";

            var redirectContext = new YahooJapanApplyRedirectContext(Context, Options, properties, authorizationEndpoint);
            Options.Provider.ApplyRedirect(redirectContext);
            return Task.FromResult<object>(null);
        }

        public override async Task<bool> InvokeAsync()
        {
            return await InvokeReplyPathAsync();
        }

        private async Task<bool> InvokeReplyPathAsync()
        {
            if(!Options.CallbackPath.HasValue || Options.CallbackPath != Request.Path)
            {
                return false;
            }

            AuthenticationTicket ticket = await AuthenticateAsync();
            if (ticket == null)
            {
                _logger.WriteWarning("Invalid return state, unable to redirect.");
                Response.StatusCode = 500;
                return true;
            }

            var context = new YahooJapanReturnEndpointContext(Context, ticket)
            {
                SignInAsAuthenticationType = Options.SignInAsAuthenticationType,
                RedirectUri = ticket.Properties.RedirectUri
            };

            await Options.Provider.ReturnEndpoint(context);

            if (context.SignInAsAuthenticationType != null && context.Identity != null)
            {
                var grantIdentity = context.Identity;
                if (!string.Equals(grantIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
                {
                    grantIdentity = new ClaimsIdentity(grantIdentity.Claims, context.SignInAsAuthenticationType, grantIdentity.NameClaimType, grantIdentity.RoleClaimType);
                }
                Context.Authentication.SignIn(context.Properties, grantIdentity);
            }

            if (!context.IsRequestCompleted && context.RedirectUri != null)
            {
                var redirectUri = context.RedirectUri;
                if (context.Identity == null)
                {
                    redirectUri = WebUtilities.AddQueryString(redirectUri, "error", "access_denied");
                }
                Response.Redirect(redirectUri);
                context.RequestCompleted();
            }

            return context.IsRequestCompleted;
        }
    }
}
