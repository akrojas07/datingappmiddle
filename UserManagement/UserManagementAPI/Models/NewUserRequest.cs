using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Models
{
    public class NewUserRequest : BaseUserRequest
    {
        public long PhotoId { get; set; }
    }
}
