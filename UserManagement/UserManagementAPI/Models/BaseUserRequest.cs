using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Models
{
    public class BaseUserRequest : BaseLoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage ="Gender is required")]
        public bool Gender { get; set; }
        public string Interests { get; set; }
        public string About { get; set; }
    }
}
