using System;
using System.Collections.Generic;

#nullable disable

namespace UserManagement.Infrastructure.Persistence.Entities
{
    public partial class User
    {
        public User()
        {
            MatchFirstUsers = new HashSet<Match>();
            MatchSecondUsers = new HashSet<Match>();
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? PhotoId { get; set; }
        public string Location { get; set; }
        public bool? Gender { get; set; }
        public string Interests { get; set; }
        public string About { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Match> MatchFirstUsers { get; set; }
        public virtual ICollection<Match> MatchSecondUsers { get; set; }
    }
}
