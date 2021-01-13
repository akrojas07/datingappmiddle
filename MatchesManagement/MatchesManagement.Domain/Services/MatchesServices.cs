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

using ApiUser = MatchesManagement.Infrastructure.UserManagementAPI.Models.User;


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
        public async Task<Match> GetMatchByMatchId(long matchId, string token)
        {
            //validate input
            if(matchId <= 0)
            {
                throw new ArgumentException();
            }

            //pull match from db
            var dbMatch = await _matchesRepository.GetMatchByMatchId(matchId);

            //validate db match is not empty
            if(dbMatch == null)
            {
                throw new Exception("No Matches exist");
            }

            //map db match to domain match
            var domainMatch = AdoMatchesMapper.DbEntityToCoreModel(dbMatch);

            //return domain match
            return domainMatch;

        }

        /// <summary>
        /// Service method to pull matches by user id
        /// </summary>
        /// <param name="userId">User ID as long</param>
        /// <returns>List of domain matches</returns>

        public async Task<List<Match>> GetMatchesByUserId(long userId, string token)
        {
            //validate input
            if(userId <= 0)
            {
                throw new ArgumentException();
            }

            var dbMatches = await _matchesRepository.GetMatchesByUserId(userId);

            //validate db matches are not empty
            if(dbMatches == null || dbMatches.Count <= 0)
            {
                throw new Exception();
            }

            //create new list of domain matches 
            List<Match> domainMatches = new List<Match>();

            //map db matches to domain matches
            foreach(var dbMatch in dbMatches)
            {
                if(dbMatch.Matched != false || dbMatch.Matched == null)
                {
                    domainMatches.Add(AdoMatchesMapper.DbEntityToCoreModel(dbMatch));
                }
                continue;
            }

            return domainMatches; 
        }

        /// <summary>
        /// Service method to pull new potential matches
        /// </summary>
        /// <param name="location">Location as string</param>
        /// <param name="token">Token as string</param>
        /// <param name="userId">User ID as long</param>
        /// <returns>List of Domain Users</returns>
        public async Task<List<User>> GetNewPotentialMatches(string location, string token, long userId)
        {
            //validate input
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException();
            }

            //create new list of domain users
            List<User> users = new List<User>();

            //call microservice method to pull users by location
            var microserviceUsers = await _userServices.GetUsersByLocation(location, token);

            //call get matches by user Id method
            var existingMatches = await _matchesRepository.GetMatchesByUserId(userId);

            //validate calls to microservice are not null or empty
            if(microserviceUsers.Count <= 0 || microserviceUsers == null)
            {
                throw new Exception();
            }

            var matchesToRemove = new List<ApiUser>();

            //if matches exist in db
            if(existingMatches.Count > 0 || existingMatches != null)
            {
                //remove from new matches list 
                foreach(var eMatch in existingMatches)
                {
                    foreach(var newMatch in microserviceUsers)
                    {
                        if(newMatch.Id == eMatch.FirstUserId)
                        {
                            matchesToRemove.Add(newMatch);
                            continue;
                        }
                        else if (newMatch.Id == eMatch.SecondUserId)
                        {
                            matchesToRemove.Add(newMatch);
                            continue;
                        }
                    }
                }
            }

            //remove existing matches
            foreach(var eMatch in matchesToRemove)
            {
                microserviceUsers.Remove(eMatch);
            }

            //map remaining micro service users to domain users
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


        /// <summary>
        /// Service Method to upsert matches
        /// </summary>
        /// <param name="matches">Domain matches as list</param>
        /// <returns>Task Complete</returns>
        public async Task UpsertMatches(List<Match> matches, string token)
        {
            //validate matches list is not null or empty
            if(matches == null || matches.Count <= 0)
            {
                throw new ArgumentException();
            }

            //validate token is not empty
            if(string.IsNullOrEmpty(token))
            {
                throw new ArgumentException();
            }

            //create new list of db matches
            var dbMatches = new List<Matches>();

            //validate matches
            var validatedMatches = await UpsertValidation(matches, token);

            if(validatedMatches.Count <= 0)
            {
                throw new Exception();
            }
            //map domain matches to db matches
            foreach(var match in validatedMatches)
            {
                dbMatches.Add(AdoMatchesMapper.CoreModelToDbEntity(match));
            }

            //call method
            await _matchesRepository.UpsertMatches(dbMatches);
        }

        /// <summary>
        /// Service method to validate matches before upserting
        /// </summary>
        /// <param name="matches">List of Domain matches</param>
        /// <param name="token">Token as string</param>
        /// <returns>List of Domain matches</returns>
        private async Task<List<Match>> UpsertValidation(List<Match> matches, string token)
        {
            //validate matches list
            if (matches == null || matches.Count <= 0)
            {
                throw new ArgumentException();
            }

            //validate token is not empty
            if(string.IsNullOrEmpty(token))
            {
                throw new ArgumentException();
            }

            var matchesToRemove = new List<Match>();

            //loop through the matches 
            foreach(var match in matches)
            {
                //validate match exists in db
                if(match.Id > 0 )
                {
                    //set value of user ids to validate
                    var userIds = new List<long>();
                    userIds.Add(match.FirstUserId);
                    userIds.Add(match.SecondUserId);


                    //pull user from db
                    var users = await _userServices.GetUsersByUserId(userIds, token);

                    //if users list is less than 2 or null
                    if(users.Count < 2 || users == null)
                    {
                        matchesToRemove.Add(match);

                        //move on to next match
                        continue;
                    }

                    //pull match from db
                    var dbMatch = await _matchesRepository.GetMatchByMatchId(match.Id);

                    //validate match exists and matched field isn't set to false
                    if(dbMatch == null || dbMatch.Matched == false)
                    {
                        //if match doesn't exist
                        matchesToRemove.Add(match);

                        //move onto next match
                        continue;
                    }

                    //if db match exists, compare to value of domain match
                    //if they match, set "matched" equal to value of liked
                    //if they don't match, set to false 
                    match.Matched = (match.Liked == dbMatch.Liked) ? match.Liked : false;
                }

                //if match doesnt exist in db, and liked field is false, set match to false
                else
                {
                    if(match.Liked == false)
                    {
                        match.Matched = false;
                    }
                }
            }

            foreach(var match in matchesToRemove)
            {
                matches.Remove(match);
            }

            return matches;

        }
    }
}
