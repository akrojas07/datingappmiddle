using System;
using System.Collections.Generic;

#nullable disable

namespace UserManagement.Infrastructure.Persistence.Entities
{
    public partial class Match
    {
        public long Id { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public bool? Liked { get; set; }
        public bool? Matched { get; set; }

        public virtual User FirstUser { get; set; }
        public virtual User SecondUser { get; set; }
    }
}
