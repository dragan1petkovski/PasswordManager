using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects.User
{
    public class UserProfile
    {
        public string email { get; set; }
        public string surname { get; set; }
        public string given_name { get; set; }
        
        public string display_name { get; set; }

        public string role { get; set; }

        public bool? mfa { get; set; }

    }
}
