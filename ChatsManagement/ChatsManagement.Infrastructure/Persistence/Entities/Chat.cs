using System;
using System.Collections.Generic;

#nullable disable

namespace ChatsManagement.Infrastructure.Persistence.Entities
{
    public partial class Chat
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public string Message { get; set; }
        public DateTime? DateSent { get; set; }

        public virtual User FirstUser { get; set; }
        public virtual Match Match { get; set; }
        public virtual User SecondUser { get; set; }
    }
}
