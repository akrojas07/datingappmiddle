using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchesManagement.API.Models
{
    public class UpsertMatchesRequest
    {
        public List<UpsertMatch> UpsertMatches { get; set; }
    }

    public class UpsertMatch
    {
        public long Id { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public bool? Liked { get; set; }
        public bool? Matched { get; set; }
    }
}
