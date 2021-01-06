using MatchesManagement.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchesManagement.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface IMatchesRepository
    {
        Task<Matches> GetMatchByMatchId(long matchId);
        Task<List<Matches>> GetMatchesByUserId(long userId);
        Task<List<Matches>> GetAllMatches();
        Task UpsertMatches(List<Matches> matches);
    }
}
