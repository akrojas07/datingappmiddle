using MatchesManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchesManagement.Domain.Interfaces
{
    public interface IMatchesServices
    {
        Task<Match> GetMatchByMatchId(long matchId);
        Task<List<Match>> GetMatchesByUserId(long userId);
        Task<List<User>> GetNewPotentialMatches(string location, string token);
        Task UpsertMatches(List<Match> matches);
    }
}
