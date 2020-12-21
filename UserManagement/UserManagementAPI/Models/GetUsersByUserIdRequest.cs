using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Models
{
    public class GetUsersByUserIdRequest
    {
        public List<long> UserIds { get; set; }
    }
}
