using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.User
{
    public class UserResponse : ItemTimeInfo
    {

        public string lastname { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string role { get; set; }

    }
}
