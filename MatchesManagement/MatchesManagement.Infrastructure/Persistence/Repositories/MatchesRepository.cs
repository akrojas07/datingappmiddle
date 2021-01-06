using MatchesManagement.Infrastructure.Persistence.Entities;
using MatchesManagement.Infrastructure.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace MatchesManagement.Infrastructure.Persistence.Repositories
{
    public class MatchesRepository : IMatchesRepository
    {
        private readonly string _connectionString;

        public MatchesRepository(IConfiguration config)
        {
            _connectionString = config.GetSection("ConnectionString:DatingDb").Value;
        }

        public async Task<List<Matches>> GetAllMatches()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var matches = await connection.QueryAsync<Matches>("[dbo].[GetAllRecordsFromMatchesTable]",  commandType: CommandType.StoredProcedure);

                connection.Close();

                return matches.ToList();
            }
        }

        public async Task<Matches> GetMatchByMatchId(long matchId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add("@MatchId", matchId);

                var match = await connection.QueryFirstOrDefaultAsync<Matches>("[dbo].[GetRecordsFromMatchesTableByMatchId]", parameter, commandType: CommandType.StoredProcedure);

                connection.Close();

                return match;
            }
        }

        public async Task<List<Matches>> GetMatchesByUserId(long userId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userId);

                var matches = await connection.QueryAsync<Matches>("dbo.GetRecordsFromMatchesTableByUserId", parameter, commandType: CommandType.StoredProcedure);

                connection.Close();

                return matches.ToList();
            }
        }

        public async Task UpsertMatches(List<Matches> matches)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                var dataTable = new DataTable();
                dataTable.Columns.Add("@MatchId", typeof(long));
                dataTable.Columns.Add("@FirstUserId", typeof(long));
                dataTable.Columns.Add("@SecondUserId", typeof(long));
                dataTable.Columns.Add("@Liked", typeof(bool));
                dataTable.Columns.Add("@Matched", typeof(bool));

                foreach(var match in matches)
                {
                    dataTable.Rows.Add(match.Id, match.FirstUserId, match.SecondUserId, match.Liked, match.Matched);
                }

                var parameter = new DynamicParameters();
                parameter.Add("@MatchChanges", dataTable.AsTableValuedParameter("[dbo].[MatchChanges]"));

                await con.ExecuteAsync("dbo.UpsertMatches", parameter, commandType: CommandType.StoredProcedure);

                con.Close();
            }
        }
    }
}
