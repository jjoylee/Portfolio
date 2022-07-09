namespace JangBoGo.Owin.Security.YahooJapan
{
    public static class Constants
    {
        public const string DefaultAuthenticationType = "YahooJapan";

        public const string AuthorizationEndpoint = "https://auth.login.yahoo.co.jp/yconnect/v2/authorization";
        public const string TokenEndpoint = "https://auth.login.yahoo.co.jp/yconnect/v2/token";
        public const string UserInformationEndpoint = "https://userinfo.yahooapis.jp/yconnect/v2/attribute";
    }
}
