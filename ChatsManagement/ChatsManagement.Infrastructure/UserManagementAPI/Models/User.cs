using System;
using System.Collections.Generic;
using System.Text;

namespace ChatsManagement.Infrastructure.UserManagementAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public bool Gender { get; set; }
        public string Interests { get; set; }
        public string About { get; set; }
        public Photo Photo { get; set; }
    }
}
