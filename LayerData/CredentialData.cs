using DataTransferObjects.Credential;
using DBLayer;
using Microsoft.EntityFrameworkCore;
using DataTransferObjects.User;


namespace LayerData
{
    public class CredentialData : Data
    {
		public CredentialData(MSSQLContext dbContext, Serilog.ILogger logger, UserSession userSession): base(dbContext,logger, userSession)
		{
		}
		
		public List<CredentialResponse> GetCredentialsByUserId(string upn, Guid clientid)
		{
			List<CredentialResponse> output = new List<CredentialResponse>();
			List<List<CredentialResponse>> temp = _dbContext.Teams.Include(t => t.users.Any(u => u.userprinciplename == upn))
							.Include(t => t.credentials)
							.Where(t => t.clientid == clientid)
							.Select(t => t.credentials.Select(c => new CredentialResponse()
																{
																	username = c.username,
																	remote = c.remote,
																	id = c.id,
																	createdate = c.createdate,
																	updatedate = c.updatedate,
																	teamid = t.id,
																	teamname = t.name,
																}
															).ToList()
									).ToList();


			foreach( List<CredentialResponse> list in temp)
			{
				output.AddRange( list );
			}
			return output;
		}
	}
}
