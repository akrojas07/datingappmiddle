using System;
using System.Collections.Generic;
using System.Text;

namespace MatchesManagement.Infrastructure.Persistence.Entities
{
    public class Matches
    {
        public long MatchId { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public bool? Liked { get; set; }
        public bool? Matched { get; set; }

    }
}
