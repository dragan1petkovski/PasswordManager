using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Membership
{
    public class MembershipRequest
    {
        public Guid id { get; set; }
        public List<Guid> activeMembers { get; set; }
    }
}
