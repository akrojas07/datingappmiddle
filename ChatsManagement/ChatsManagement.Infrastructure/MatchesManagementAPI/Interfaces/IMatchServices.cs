using ChatsManagement.Infrastructure.MatchesManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatsManagement.Infrastructure.MatchesManagementAPI.Interfaces
{
    public interface IMatchServices
    {
        //get match by match id
        Task<Match> GetMatchByMatchId(long matchId, string token);
    }
}
