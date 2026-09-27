using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Team
{
    public class TeamResponse : ItemId
    {
        public Guid clientid { get; set; }
        public string clientname { get; set; }
    }
}
