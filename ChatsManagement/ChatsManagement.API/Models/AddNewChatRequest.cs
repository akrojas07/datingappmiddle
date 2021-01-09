using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatsManagement.API.Models
{
    public class AddNewChatRequest
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
    }
}
