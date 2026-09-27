using ApplicationStatusCode;
using LayerData;
using DataTransferObjects.Credential;
using DomainModel;
using Services.Audit;
using System.Text;

namespace Services
{
    public class SvcCredential
    {
        private readonly CredentialData _credentialData;
        private readonly SvcEncryption _svcEncryption;
        private readonly UserSession _userSession;
        public SvcCredential(CredentialData credentialData, SvcEncryption svcEncryption, UserSession userSession)
        {
            _credentialData = credentialData;
            _svcEncryption = svcEncryption;
            _userSession = userSession;
        }

        public AppStatusCode<Credential> Update(CredentialRequest update, Guid id)
        {
            Credential _credential = _credentialData.Read<Credential>(id);
            if(_credential is null || _credential.teamid != update.teamid)
            {
                return AppStatusCode<Credential>.ItemDontExist;
            }
            _credential.username = update.username;
            _credential.remote = update.remote;
            
            if(update.password is null)
            {
                _credential.password = null;
            }
            else if(update.password != string.Empty)
            {
                _credential.password = _svcEncryption.Encrypt(Encoding.UTF8.GetBytes(update.password));
            }

            if(update.key is null)
            {
                _credential.key = null;
            }
            else if(update.key != string.Empty)
            {
                _credential.key = _svcEncryption.Encrypt(Encoding.UTF8.GetBytes(update.key));
            }

            _credential.updatedate = DateTime.Now;

            if(_credentialData.Update<Credential>(_credential))
            {
                return AppStatusCode<Credential>.UpdateItem;
            }
            else
            {
                return AppStatusCode<Credential>.ServiceUnavailable;
            }
        }

        public List<CredentialResponse> GetAllCredentials(Guid clientid)
        {
            return _credentialData.Read<Credential>(c => c.team)
                                    .Where(cred => cred.team.clientid == clientid && cred.team.users.Any(u => u.userprinciplename == _userSession.upn))
                                    .Select(item => new CredentialResponse()
                                    {
                                        id = item.id,
                                        username = item.username,
                                        remote = item.remote,
                                        createdate = item.createdate,
                                        updatedate = item.updatedate,
                                        teamid = item.team.id,
                                        teamname = item.team.name,
                                        key = item.key != null? true : false,
                                        password = item.password != null?true : false
                                        
                                    })
                                    .ToList();
            
        }

        public string GetPassword(Guid teamid, Guid id)
        {
            Credential cred = _credentialData.Read<Credential>().Where(c => c.teamid == teamid && c.team.users.Any(u => u.userprinciplename == _userSession.upn)).FirstOrDefault(c => c.id == id);
            if(cred == null)
            {
                return null;
            }

            return Encoding.UTF8.GetString(_svcEncryption.Decrypt(cred.password));
                
        }

        public AppStatusCode<Credential> Create(CredentialRequest request)
        {
            bool isSuccessful = _credentialData.Write<Credential>(new Credential()
            {
                id = Guid.NewGuid(),
                username = request.username,
                remote = request.remote,
                createdate = DateTime.Now,
                updatedate = DateTime.Now,
                password = (string.IsNullOrEmpty(request.password) || string.IsNullOrWhiteSpace(request.password)) ? null : _svcEncryption.Encrypt(Encoding.UTF8.GetBytes(request.password)),
                key = (string.IsNullOrEmpty(request.key)||string.IsNullOrWhiteSpace(request.key))?null:_svcEncryption.Encrypt(Encoding.UTF8.GetBytes(request.key)),
                teamid = request.teamid,
            });
            if (isSuccessful)
            {
                return AppStatusCode<Credential>.AddNewItem;
            }
            else
            {
                return AppStatusCode<Credential>.ServiceUnavailable;
            }

        }

        public byte[] GetKey(Guid teamid, Guid id)
        {
            Credential cred = _credentialData.Read<Credential>().Where(c => c.teamid == teamid && c.team.users.Any(u => u.userprinciplename == _userSession.upn)).FirstOrDefault(c => c.id == id);
            if (cred == null)
            {
                return null;
            }

            return _svcEncryption.Decrypt(cred.key);
        }

        public byte[] GetKeyEncrypted(Guid teamid, Guid id)
        {
            Credential cred = _credentialData.Read<Credential>().Where(c => c.teamid == teamid && c.team.users.Any(u => u.userprinciplename == _userSession.upn)).FirstOrDefault(c => c.id == id);
            if (cred == null)
            {
                return null;
            }

            return cred.key;
        }

        public AppStatusCode<Credential> Delete(Guid teamid, Guid id)
        {
            Credential cred = _credentialData.Read<Credential>().Where(c => c.teamid == teamid && c.team.users.Any(u => u.userprinciplename == _userSession.upn)).FirstOrDefault(c => c.id == id);
            if (cred == null)
            {
                return AppStatusCode<Credential>.ItemDontExist;
            }
            else
            {
                if(_credentialData.Delete<Credential>(cred))
                {
                    return AppStatusCode<Credential>.DeleteItem;
                }
                return AppStatusCode<Credential>.ServiceUnavailable;
            }
        }
    }
}
