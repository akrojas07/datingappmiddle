using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure.Http
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<T> GetAsync<T>(string url, string key, string errorMessage = "")
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);

            var result = await httpClient.GetAsync(url);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(errorMessage);
            }

            var objStringResponse = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(objStringResponse);
        }
    }
}
