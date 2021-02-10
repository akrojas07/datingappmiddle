using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatsManagement.Infrastructure.HTTPClient
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url, string key, string scheme = "", string errorMessage = "");
        Task<T> PutAsync<T>(string url, string key, string data, string scheme = "", string errorMessage = "");
    }
}
