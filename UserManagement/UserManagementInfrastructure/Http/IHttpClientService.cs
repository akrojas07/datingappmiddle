using System.Threading.Tasks;

namespace UserManagement.Infrastructure.Http
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url, string key, string errorMessage = "");
    }
}
