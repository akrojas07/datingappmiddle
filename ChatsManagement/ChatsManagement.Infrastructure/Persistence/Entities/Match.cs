﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ChatsManagement.Infrastructure.Persistence.Entities
{
    public partial class Match
    {
        public Match()
        {
            Chats = new HashSet<Chat>();
        }

        public long Id { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public bool? Liked { get; set; }
        public bool? Matched { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual User FirstUser { get; set; }
        public virtual User SecondUser { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}
