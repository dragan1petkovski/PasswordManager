using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Membership
{
    public class MembershipResponse
    {
        public Guid id { get; set; }
        public List<MemberItem> nonMembers { get; set; }
        public List<MemberItem> members { get; set; }
    }
}
