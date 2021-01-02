using MatchesManagement.API.Models;
using MatchesManagement.Domain.Interfaces;
using MatchesManagement.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MatchesManagement.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("matches")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesServices _matchesServices;

        public MatchesController(IMatchesServices matchesServices)
        {
            _matchesServices = matchesServices;
        }

        [HttpGet]
        [Route("newmatches/{userId}")]
        public async Task<IActionResult> GetNewMatches(long userId, [FromQuery]string location)
        {
            //validate inputs
            if (userId <= 0)
            {
                return StatusCode(400);
            }

            if (string.IsNullOrEmpty(location))
            {
                return StatusCode(400);
            }

            var token = "";

            if (Request.Headers.ContainsKey("Authorization"))
            {
                var jwt = (Request.Headers.FirstOrDefault(s => s.Key.Equals("Authorization"))).Value;

                if (jwt.Count <= 0)
                {
                    return StatusCode(400);
                }

                token = jwt[0].Replace("Bearer ", "");
            }

            try
            {
                var matches = await _matchesServices.GetNewPotentialMatches(location, token, userId);
                return StatusCode(200, matches);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet]
        [Route("match/{matchId}")]
        public async Task<IActionResult> GetMatchesByMatchId([FromRoute] long matchId)
        {
            if(matchId <= 0)
            {
                return StatusCode(400);
            }
            var token = "";

            if (Request.Headers.ContainsKey("Authorization"))
            {
                var jwt = (Request.Headers.FirstOrDefault(s => s.Key.Equals("Authorization"))).Value;

                if (jwt.Count <= 0)
                {
                    return StatusCode(400);
                }

                token = jwt[0].Replace("Bearer ", "");
            }

            try
            {
                var match = await _matchesServices.GetMatchByMatchId(matchId, token);
                return StatusCode(200, match);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetMatchesByUserId(long userId)
        {
            if(userId <= 0)
            {
                return StatusCode(400);
            }

            var token = "";

            if (Request.Headers.ContainsKey("Authorization"))
            {
                var jwt = (Request.Headers.FirstOrDefault(s => s.Key.Equals("Authorization"))).Value;

                if (jwt.Count <= 0)
                {
                    return StatusCode(400);
                }

                token = jwt[0].Replace("Bearer ", "");
            }

            try
            {
                var matches = await _matchesServices.GetMatchesByUserId(userId, token);
                return StatusCode(200, matches);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpsertMatches([FromBody] UpsertMatchesRequest upsertMatchesRequest)
        {
            if(upsertMatchesRequest == null)
            {
                return StatusCode(400);
            }

            foreach(var match in upsertMatchesRequest.UpsertMatches)
            {
                if(match.FirstUserId <= 0 || match.SecondUserId <= 0 || match.Liked == null)
                {
                    return StatusCode(400);
                }
            }

            var token = "";

            if (Request.Headers.ContainsKey("Authorization"))
            {
                var jwt = (Request.Headers.FirstOrDefault(s => s.Key.Equals("Authorization"))).Value;

                if (jwt.Count <= 0)
                {
                    return StatusCode(400);
                }

                token = jwt[0].Replace("Bearer ", "");
            }

            try
            {
                var domainMatches = new List<Match>();

                foreach(var upsertMatch in upsertMatchesRequest.UpsertMatches)
                {
                    var domainMatch = new Match()
                    {
                        Id = upsertMatch.Id,
                        FirstUserId = upsertMatch.FirstUserId,
                        SecondUserId = upsertMatch.SecondUserId,
                        Liked = upsertMatch.Liked,
                        Matched = upsertMatch.Matched
                    };

                    domainMatches.Add(domainMatch);
                }

                await _matchesServices.UpsertMatches(domainMatches, token);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
