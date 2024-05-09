using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI
{
    internal class PostRequest
    {
        AuthToken token;

        private readonly string apiUrl = "https://api.huntflow.ru/v2";
        private readonly string authorizationToken = $"Bearer ";

        public PostRequest(AuthToken token)
        {
            this.token = token ?? throw new ArgumentNullException(nameof(token));
            authorizationToken += token.access_token;
        }

        public string RefreshToken()
        {
            return SendRequest(apiUrl + "/token/refresh", $"{{\r\n  \"refresh_token\": \"{token.refresh_token}\"\r\n}}").Result;
        }

        public Task<string> SendRequest(string url, string requestBody)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Authorization", authorizationToken);
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Host", "api.huntflow.ru");
            request.Headers.Add("Referer", "https://api.huntflow.ru/v2/docs");

            /*
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    var args = header.Split(':');
                    request.Headers.Add(args[0].Trim(), args[1].Trim());
                }
            }
            */

            HttpResponseMessage response = client.PostAsync(url, new StringContent(requestBody)).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
    }
}
