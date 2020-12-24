using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchesManagement.Infrastructure.HTTPClient
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url, string key, string errorMessage = "");
        Task<T> PutAsync<T>(string url, string key, string data, string errorMessage = "");
    }
}
