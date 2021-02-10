using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatsManagement.API.Models
{
    public class AddNewChatRequest
    {
        public long Id { get; set; }

        [Required]
        public long MatchId { get; set; }

        [Required]
        public long FirstUserId { get; set; }

        [Required]
        public long SecondUserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Message Required")]

        public string Message { get; set; }
        public DateTime DateSent { get; set; }
    }
}
