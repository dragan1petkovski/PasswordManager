using LayerData;
using DomainModel;
using ApplicationStatusCode;
using DataTransferObjects.Team;
using DataTransferObjects.Membership;
using DataTransferObjects;

namespace Services
{
	public class SvcTeam
	{
		private readonly TeamData _teamData;
		private readonly Serilog.ILogger _logger;
		
		public SvcTeam(TeamData teamData, Serilog.ILogger logger)
		{
			this._teamData = teamData;
			this._logger = logger;
		}

		public List<TeamResponseDetails> GetAllDetails()
		{
            return _teamData.Read<Team>(t => t.client).Select(team => new TeamResponseDetails()
            {
                clientid = team.clientid,
                clientname = team.client.name,
                name = team.name,
                id = team.id,
                createdate = team.createdate,
                updatedate = team.updatedate
            }).ToList();
        }

		public List<TeamResponse> GetAll()
		{
            return _teamData.Read<Team>(t => t.client).Select(t => new TeamResponse() { id = t.id, name = t.name, clientid = t.clientid, clientname = t.client.name }).ToList();
        }

		public Team GetById(Guid id)
		{
			return _teamData.Read<Team>(id);
		}

		public List<ItemId> GetTeamsByClientId(Guid clientid, string upn)
		{
            return _teamData.Read<Team>()
                                .Where(t => t.clientid == clientid && t.users.Any(u => u.userprinciplename == upn))
                                .Select(t => new ItemId() { id = t.id, name = t.name }).ToList();


        }

		public List<Team> GetTeamsByUserId(string upn)
		{
			return _teamData.Read<Team>().Where(t => t.users.Any(u => u.userprinciplename == upn)).ToList();
		}

		public AppStatusCode<Team> Create(TeamRequest newTeam)
		{
			Client _client = _teamData.GetClientById(newTeam.clientid);
			if(_client is null)
			{
				return AppStatusCode<Team>.ItemDontExist;
			}
			Team _newTeam = new Team()
			{
				id = Guid.NewGuid(),
				name = newTeam.name,
				clientid = _client.id,
				client = _client,
				users = new List<User>(),
				createdate = DateTime.Now,
				updatedate = DateTime.Now,
			};

			if (_teamData.Write<Team>(_newTeam))
			{
				return AppStatusCode<Team>.AddNewItem;
			}
			else
			{
				return AppStatusCode<Team>.ServiceUnavailable;
			}


		}

		public AppStatusCode<Team> Delete(Guid id)
		{
			Team item = _teamData.Read<Team>(id);
			if (item == null)
			{
				return AppStatusCode<Team>.ItemDontExist;
			}
			item.users = new List<User>();
			if (_teamData.Delete(item))
			{
				return AppStatusCode<Team>.DeleteItem;
			}
			return AppStatusCode<Team>.ServiceUnavailable;
			
			
		}
	
		public AppStatusCode<Team> Update(TeamRequest updateTeam, Guid teamid)
		{
			Team team = _teamData.Read<Team>(t => t.client).FirstOrDefault(t => t.id == teamid);
			if(team is null)
			{
				return AppStatusCode<Team>.ItemDontExist;
			}

			if(team.client.id != updateTeam.clientid)
			{
				return AppStatusCode<Team>.ItemDontExist;
			}

			team.name = updateTeam.name;
			team.updatedate = DateTime.Now;

			if(_teamData.Update<Team>(team))
			{
                return AppStatusCode<Team>.UpdateItem;
            }
			else
			{
				return AppStatusCode<Team>.ServiceUnavailable;
			}
			

		}

		public MembershipResponse GetMembershipData(Guid teamid)
		{
			return _teamData.GetMembershipData(teamid);
		}

		public AppStatusCode<Team> SetMembershipData(MembershipRequest membership)
		{
			Team team = _teamData.Read<Team>(t => t.users).FirstOrDefault(t => t.id == membership.id);
			if (team == null)
			{
				return AppStatusCode<Team>.ItemDontExist;
			}
			List<User> users;
			if (membership.activeMembers.Count == 0)
			{
				users = new List<User>();
			}
			else
			{
				users = _teamData.Read<User>().Where(u => membership.activeMembers.Contains(u.id)).ToList();
			}
			if (users.Count == 0 && membership.activeMembers.ToList().Count > 0)
			{
				return AppStatusCode<Team>.InvalidRequest;
			}
			team.users = users;
			if (_teamData.Update<Team>(team))
			{
				return AppStatusCode<Team>.SuccessfulMembershipUpdate;
			}
			else
			{
				return AppStatusCode<Team>.FailedMembershipUpdate;
			}

		}
	}
}
