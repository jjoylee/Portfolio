using Newtonsoft.Json.Linq;

namespace Lib.Owin.Security.YahooJapan.Model
{
    public class UserInfoResponse
    {
        public UserInfoResponse(JObject response)
        {
            Response = response;
            Id = TryGetValue("sub");
            Email = TryGetValue("email");
        }

        public JObject Response { get; }
        public string Id { get; }
        public string Email { get; }

        private string TryGetValue(string propertyName)
        {
            JToken value;
            return Response.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }
    }
}
