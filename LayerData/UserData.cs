using DataTransferObjects.Membership;
using DBLayer;
using Microsoft.EntityFrameworkCore;
using DataTransferObjects.User;

namespace LayerData
{
    public class UserData : Data
    {
		public UserData(MSSQLContext dbContext, Serilog.ILogger logger, UserSession userSession) : base(dbContext, logger, userSession)
		{

		}

        public MembershipResponse GetMembershipData(Guid userid)
        {
            var dbquery = _dbContext.Teams.Include(t => t.client);

            var members = dbquery.Where(t => t.users.Any(u => u.id == userid)).Select(t => new MemberItem()
            {
                id = t.id,
                name = $"{t.client.name} - {t.name}",

            });

            var nonMembers = dbquery.Where(t => t.users.All(u => u.id != userid)).Select(t => new MemberItem()
            {
                id = t.id,
                name = $"{t.client.name} - {t.name}"

            });

            return new MembershipResponse()
            {
                id = userid,
                members = members.ToList(),
                nonMembers = nonMembers.ToList()
            };
        }
    }
}
