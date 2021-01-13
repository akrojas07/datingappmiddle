using ChatsManagement.Infrastructure.HTTPClient;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatsManagement.Infrastructure.MatchesManagementAPI.Services
{
    public class MatchServices : IMatchServices
    {
        private readonly string _baseUrl;
        private readonly IHttpClientService _httpService;

        public MatchServices(IConfiguration configuration, IHttpClientService httpService)
        {
            _baseUrl = configuration.GetSection("MatchesManagement:Url").Value;
            _httpService = httpService;
        }

        public async Task<Match> GetMatchByMatchId(long matchId, string token)
        {
            return await _httpService.GetAsync<Match>($"{_baseUrl}/matches/match/{matchId}", token, "Bearer");
        }
    }
}
