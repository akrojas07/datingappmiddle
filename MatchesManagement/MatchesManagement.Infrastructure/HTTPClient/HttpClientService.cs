using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace MatchesManagement.Infrastructure.HTTPClient
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<T> GetAsync<T>(string url, string key, string errorMessage = "")
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);

            var result = await httpClient.GetAsync(url);

            if(result.StatusCode != HttpStatusCode.OK)
            {
                var error = (!string.IsNullOrEmpty(errorMessage))
                ? errorMessage
                : await result.Content.ReadAsStringAsync();

                throw new Exception(error);
            }

            var objStringResponse = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(objStringResponse);

        }

        public async Task<T> PutAsync<T>(string url, string key, string data, string errorMessage = "")
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);

            var body = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await httpClient.PutAsync(url, body);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                var error = (!string.IsNullOrEmpty(errorMessage))
                    ? errorMessage
                    : await result.Content.ReadAsStringAsync();

                throw new Exception(error);
            }

            var objStringResponse = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(objStringResponse);
        }
    }
}
