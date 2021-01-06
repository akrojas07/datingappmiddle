using System;
using System.Collections.Generic;
using System.Text;

namespace MatchesManagement.Domain.Models
{
    public class Match
    {
        public long Id { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public bool? Liked { get; set; }
        public bool? Matched { get; set; }
    }
}
