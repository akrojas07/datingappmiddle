using MatchesManagement.Domain.Interfaces;
using MatchesManagement.Domain.Models;
using MatchesManagement.Infrastructure.Persistence.Repositories.Interfaces;
using MatchesManagement.Infrastructure.UserManagementAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MatchesManagement.Domain.AdoMapper;
using MatchesManagement.Infrastructure.Persistence.Entities;

namespace MatchesManagement.Domain.Services
{
    public class MatchesServices : IMatchesServices
    {
        private readonly IMatchesRepository _matchesRepository;
        private readonly IUserServices _userServices;
        public MatchesServices(IMatchesRepository matchesRepository, IUserServices userServices)
        {
            _matchesRepository = matchesRepository;
            _userServices = userServices;
        }

        /// <summary>
        /// Service method to pull single match by match id
        /// </summary>
        /// <param name="matchId">Long</param>
        /// <returns>Domain match model</returns>
        public async Task<Match> GetMatchByMatchId(long matchId)
        {
            //validate input
            if(matchId <0)
            {
                throw new ArgumentException();
            }

            //pull match from db
            var dbMatch = await _matchesRepository.GetMatchByMatchId(matchId);

            //validate db match is not empty
            if(dbMatch == null)
            {
                throw new Exception();
            }

            //map db match to domain match
            var domainMatch = AdoMatchesMapper.DbEntityToCoreModel(dbMatch);

            //return domain match
            return domainMatch;

        }

        public async Task<List<Match>> GetMatchesByUserId(long userId)
        {
            //validate input
            if(userId < 0)
            {
                throw new ArgumentException();
            }

            var dbMatches = await _matchesRepository.GetMatchesByUserId(userId);

            //validate db matches are not empty
            if(dbMatches == null || dbMatches.Count < 0)
            {
                throw new Exception();
            }

            //create new list of domain matches 
            List<Match> domainMatches = new List<Match>();

            //map db matches to domain matches
            foreach(var dbMatch in dbMatches)
            {
                domainMatches.Add(AdoMatchesMapper.DbEntityToCoreModel(dbMatch));
            }

            return domainMatches; 
        }

        public async Task<List<User>> GetNewPotentialMatches(string location, string token, long userId)
        {
            //validate input
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException();
            }

            //create new list of domain users
            List<User> users = new List<User>();

            //call microservice method
            var microserviceUsers = await _userServices.GetUsersByLocation(location, token);

            //map micro service users to domain users
            foreach(var msUser in microserviceUsers)
            {
                //validate current user isn't being included in matches list
                if (msUser.Id == userId)
                {
                    continue;
                }
                
                users.Add(AdoUserMapper.MsUserToDomainUser(msUser));

            }

            return users;
        }
        
        public async Task UpsertMatches(List<Match> matches)
        {
            //validate matches list
            if(matches == null || matches.Count < 0)
            {
                throw new ArgumentException();
            }

            var dbMatches = new List<Matches>();
            //map domain matches to db matches
            foreach(var match in matches)
            {
                dbMatches.Add(AdoMatchesMapper.CoreModelToDbEntity(match));
            }

            //call method
            await _matchesRepository.UpsertMatches(dbMatches);
        }
    }
}
