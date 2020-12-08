using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Models
{
    public class UpdateUserRequest : BaseUserRequest
    {
        public long UserId { get; set; }
    }
}
