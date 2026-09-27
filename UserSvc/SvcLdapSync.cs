using ApplicationStatusCode;
using LayerData;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.DirectoryServices;

namespace Services
{
    public class SvcLdapSync
    {
        private readonly IConfiguration _config;
        private Serilog.ILogger _logger;
        private readonly UserData _userdata;

        public SvcLdapSync(IConfiguration config, ILogger logger, UserData userData)
        {
            _config = config;
            _logger = logger;
            _userdata = userData;
        }

        private List<User> GetLDAPUsers(string ADServer, string groupDN, string role)
        {
            List<User> output = new List<User>();
            using DirectoryEntry entry = new DirectoryEntry($"ldap://{ADServer}");
            using DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = $"(&(objectClass=user)(memberOf={groupDN}))";
            searcher.PropertiesToLoad.Add("givenName");
            searcher.PropertiesToLoad.Add("userPrincipalName");
            searcher.PropertiesToLoad.Add("sAMAccountName");
            searcher.PropertiesToLoad.Add("mail");
            searcher.PropertiesToLoad.Add("sn");

            SearchResultCollection results;
            try
            {
                results = searcher.FindAll();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to connect to AD for user Sync \n{ex.Message}");
                return null;
            }
            foreach (SearchResult res in results)
            {
                var givenName = res.Properties["givenName"];
                var surname = res.Properties["sn"];
                var upn = res.Properties["userPrincipalName"];
                var mail = res.Properties["mail"];
                var sam = res.Properties["sAMAccountName"];

                if (givenName.Count == 1 && surname.Count == 1 && upn.Count == 1 && mail.Count == 1 && sam.Count == 1)
                {
                    output.Add(new User()
                    {
                        email = mail[0].ToString(),
                        userprinciplename = upn[0].ToString(),
                        username = sam[0].ToString(),
                        firstname = givenName[0].ToString(),
                        lastname = surname[0].ToString(),
                        createdate = DateTime.Now,
                        updatedate = DateTime.Now,
                        role = role

                    });
                }
                else
                {
                    Console.WriteLine("The user must have fullowing AD Attributes to be valid user: givenName, sn, userPrincipleName, mail, and sAMAccountName");
                }
            }
            return output;

        }


        public AppStatusCode<User> ADUserSync()
        {
            string adServer = _config.GetSection("LdapSettings")["ADServer"];
            string cm_user = _config.GetSection("LdapSettings")["DistinguishedNameADUserGroup"];
            string cm_admim = _config.GetSection("LdapSettings")["DistingueshedNameADAdminGroup"];

            //Web App need to work under sMSA Account or gMSA Account in order for ldap sync to work
            List<User> ldapUsers = GetLDAPUsers(adServer, cm_user, "User");
            List<User> ldapAdmins = GetLDAPUsers(adServer, cm_admim, "Admin");
            List<User> allLDAPUsers = new List<User>();
            List<User> removeUser = new List<User>();
            if (ldapUsers == null || ldapAdmins == null)
            {
                return AppStatusCode<User>.ADSyncFailed;
            }
            allLDAPUsers.AddRange(ldapUsers);
            allLDAPUsers.AddRange(ldapAdmins);

            IEnumerable<User> allDbUsers = _userdata.Read<User>();
            if (allDbUsers.Count() == 0)
            {
                _userdata.Write<User>(allLDAPUsers.Select(u =>
                {
                    u.id = Guid.NewGuid();
                    u.createdate = DateTime.Now;
                    u.updatedate = DateTime.Now;
                    return u;
                }).ToList());
            }
            if (allLDAPUsers.Count() > allDbUsers.Count())
            {
                //Add and modify users from DB
                //New Users added
                _userdata.Write<User>(allLDAPUsers.Where(u => allDbUsers.All(dbu => dbu.userprinciplename != u.userprinciplename)).Select(u =>
                {
                    u.id = Guid.NewGuid();
                    u.createdate = DateTime.Now;
                    u.updatedate = DateTime.Now;
                    return u;
                }).ToList());

                //Modify Existing users
            }
            else if (allLDAPUsers.Count() < allDbUsers.Count())
            {
                //Remove and modify users from DB
            }
            else
            {
                //only edit users
                //check if all users are same
                //if not remove from db and add new ones 
                //if same only modify db users 
            }
            return AppStatusCode<User>.ADSyncSuccess;
        }

    }
}
