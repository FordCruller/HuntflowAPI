using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class AuthToken
    {
        
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int refresh_token_expires_in { get; set; }
        public string refresh_token { get; set; }

        public AuthToken(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}