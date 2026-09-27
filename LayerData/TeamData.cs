using DataTransferObjects.Membership;
using DBLayer;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using Services.Audit;

namespace LayerData
{
    public class TeamData : Data
    {
		public TeamData(MSSQLContext dbContext, Serilog.ILogger logger, UserSession userSession) : base(dbContext, logger, userSession)
		{

		}
        public Client GetClientById(Guid id)
        {
            try
            {
                return _dbContext.Clients.FirstOrDefault(c => c.id == id);
            }
            catch (Exception er)
            {
                _logger.Error($"Failed to get Client by ID: {id}\n{er.Message}");
                return null;

            }
        }

        public MembershipResponse GetMembershipData(Guid teamid)
        {
            var dbquery = _dbContext.Users.Include(u => u.teams).Where(u => u.role == "User");

            var active = dbquery.Where(u => u.teams.Any(t => t.id == teamid)).Select(u => new MemberItem()
            {
                id = u.id,
                name = $"{u.fullName}"

            });

            var nonMembers = dbquery.Where(u => u.teams.All(t => t.id != teamid)).Select(u => new MemberItem()
            {
                id = u.id,
                name = $"{u.fullName}",

            });



            return new MembershipResponse()
            {
                id = teamid,
                members = active.ToList(),
                nonMembers = nonMembers.ToList()
            };
        }
    }
}
