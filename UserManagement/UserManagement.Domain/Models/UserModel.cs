﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public bool Gender { get; set; }
        public string Interests { get; set; }
        public string About { get; set; }
        public DomainPhoto Photo { get; set; }
    }
}
