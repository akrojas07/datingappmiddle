using MatchesManagement.Infrastructure.HTTPClient;
using MatchesManagement.Infrastructure.UserManagementAPI.Interfaces;
using MatchesManagement.Infrastructure.UserManagementAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchesManagement.Infrastructure.UserManagementAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly string _baseUrl;
        private readonly IHttpClientService _httpService;

        public UserServices(IConfiguration config, IHttpClientService httpClientService)
        {
            _baseUrl = config.GetSection("UserManagement:URL").Value;
            _httpService = httpClientService;
        }

        public async Task<List<User>> GetUsersByLocation(string location, string token)
        {
            return await _httpService.GetAsync<List<User>>($"{_baseUrl} + user/{location}", token);
        }

        public async Task<List<User>> GetUsersByUserId(List<long> userIds, string token)
        {
            var data = JsonConvert.SerializeObject(userIds);
            return await _httpService.PutAsync<List<User>>($"{_baseUrl} + user/", $"Bearer {token}", data);
        }


    }
}
