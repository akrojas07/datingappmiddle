using System;
using System.Collections.Generic;
using System.Text;

namespace ChatsManagement.Domain.Models
{
    public class DomainChat
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
    }
}
