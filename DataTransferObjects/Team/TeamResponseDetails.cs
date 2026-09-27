using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Team
{
    public class TeamResponseDetails : ItemTimeInfo
    {
        public string clientname { get; set;  }
        public Guid clientid { get; set; }
    }
}
