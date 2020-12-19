using System;
using System.Collections.Generic;

#nullable disable

namespace UserManagement.Infrastructure.Persistence.Entities
{
    public partial class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? PhotoId { get; set; }
    }
}
