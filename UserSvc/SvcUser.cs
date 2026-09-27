using ApplicationStatusCode;
using LayerData;
using DataTransferObjects.Membership;
using DataTransferObjects.User;
using DomainModel;
using Serilog;
namespace Services
{
    public class SvcUser
    {
        private Serilog.ILogger _logger;
        private readonly UserData _userdata;

        public SvcUser(ILogger logger, UserData userData)
        {
            _logger = logger;
            _userdata = userData;
        }

        public IEnumerable<UserResponse> GetAllUsers()
        {
            return _userdata.Read<User>().Select(u => new UserResponse()
            {
                name = u.firstname,
                lastname = u.lastname,
                email = u.email,
                username = u.username,
                id = u.id,
                role = u.role,
                createdate = u.createdate,
                updatedate = u.updatedate
            });
            
        }

        private User UpdateDBUser(User ldapUser, User dbUser)
        {
            if (dbUser.email != ldapUser.email)
            {
                dbUser.email = ldapUser.email;
            }
            if (dbUser.firstname != ldapUser.firstname)
            {
                dbUser.firstname = ldapUser.firstname;
            }
            if (dbUser.lastname != ldapUser.lastname)
            {
                dbUser.lastname = ldapUser.lastname;
            }
            if (dbUser.username != ldapUser.username)
            {
                dbUser.username = ldapUser.username;
            }

            return dbUser;
        }


    
    
        public MembershipResponse GetMembershipData(Guid userid)
        {
            try
            {
                return _userdata.GetMembershipData(userid);
            }
            catch (Exception err)
            {
                _logger.Error($"Problem geting items from database:\n{err.Message}");
                return null;
            }
        }

        public AppStatusCode<User> SetMembershipData(MembershipRequest membership)
        {
            User user = _userdata.Read<User>(u => u.teams).FirstOrDefault(u => u.id == membership.id);
            if (user == null)
            {
                return AppStatusCode<User>.ItemDontExist;
            }
            List<Team> teams;
            if (membership.activeMembers.Count == 0)
            {
                teams = new List<Team>();
            }
            else
            {
                teams = _userdata.Read<Team>().Where(t => membership.activeMembers.Contains(t.id)).ToList();

            }
            if (teams.Count == 0 && membership.activeMembers.ToList().Count > 0)
            {
                return AppStatusCode<User>.InvalidRequest;
            }
            user.teams = teams;
            if (_userdata.Update<User>(user))
            {
                return AppStatusCode<User>.SuccessfulMembershipUpdate;
            }
            else
            {
                return AppStatusCode<User>.FailedMembershipUpdate;
            }

        }
    }
}
