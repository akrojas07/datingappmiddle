using MatchesManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchesManagement.Domain.Interfaces
{
    public interface IMatchesServices
    {
        Task<Match> GetMatchByMatchId(long matchId, string token);
        Task<List<Match>> GetMatchesByUserId(long userId, string token);
        Task<List<User>> GetNewPotentialMatches(string location, string token, long userId);
        Task UpsertMatches(List<Match> matches, string token);
    }
}
