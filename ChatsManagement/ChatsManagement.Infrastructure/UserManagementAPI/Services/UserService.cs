using ChatsManagement.Infrastructure.HTTPClient;
using ChatsManagement.Infrastructure.UserManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.UserManagementAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatsManagement.Infrastructure.UserManagementAPI.Services
{
    public class UserService: IUserService
    {
        private readonly string _baseUrl;
        private readonly IHttpClientService _httpService;

        public UserService(IConfiguration config, IHttpClientService httpClientService)
        {
            _baseUrl = config.GetSection("UserManagement:Url").Value;
            _httpService = httpClientService;
        }

        public async Task<List<User>> GetUsersByUserId(List<long> userIds, string token)
        {
            var data = new { UserIds = userIds };

            return await _httpService.PutAsync<List<User>>($"{_baseUrl}/user", token, JsonConvert.SerializeObject(data), "Bearer");
        }

    }
}
