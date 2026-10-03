
using LayerData;
using ApplicationStatusCode;
using DataTransferObjects.Client;
using DomainModel;

namespace Services
{
	public class SvcClient
	{
		private readonly ClientData _clientData;
		private Serilog.ILogger _logger;

		public SvcClient(ClientData clientData, Serilog.ILogger logger)
		{
			_clientData = clientData;
			_logger = logger;
		}


		public IEnumerable<ClientResponse> GetClientResponse()
		{
            return _clientData.Read<Client>().Select(item => new ClientResponse()
            {
                id = item.id,
                name = item.name
            });
        }

        public IEnumerable<ClientResponse> GetClientResponse(string upn)
        {
            return _clientData.Read<Client>().Where(c => c.teams.Any(t => t.users.Any(u => u.userprinciplename == upn))).Select(item => new ClientResponse()
            {
                id = item.id,
                name = item.name
            });
        }

        public IEnumerable<ClientDetailResponse> GetClientDetailsResponse()
		{
            return _clientData.Read<Client>().Select(item => new ClientDetailResponse()
            {
                id = item.id,
                name = item.name,
                createdate = item.createdate,
                updatedate = item.updatedate
            });
        }

		public IEnumerable<ClientDetailResponse> GetClientDetailsResponse(string upn)
		{
			return _clientData.Read<Client>().Where(c => c.teams.Any(t => t.users.Any(u => u.userprinciplename == upn))).Select(item => new ClientDetailResponse()
            {
                id = item.id,
                name = item.name,
                createdate = item.createdate,
                updatedate = item.updatedate
            });
		}



		public AppStatusCode<Client> Create(ClientRequest item)
		{
			if(_clientData.Read<Client>().Any(c => c.name.ToLower() == item.name.ToLower()))
			{
				_logger.Debug($"Client already exist {item.name}");
				return AppStatusCode<Client>.ItemExist;
			}
			bool isCreated = _clientData.Write<Client>(new Client()
			{
				id = Guid.NewGuid(),
				name = item.name.ToLower(),
				createdate = DateTime.Now,
				updatedate = DateTime.Now
			});
			if (isCreated)
			{
				return AppStatusCode<Client>.AddNewItem;
			}
			return AppStatusCode<Client>.ServiceUnavailable;
		}

		public AppStatusCode<Client> Delete(Guid id)
		{
			Client item = _clientData.Read<Client>(id);

			if(item == null)
			{
				return AppStatusCode<Client>.ItemDontExist;
			}
			if(_clientData.Delete(item))
			{
				return AppStatusCode<Client>.DeleteItem;
			}
			return AppStatusCode<Client>.ServiceUnavailable;
		}
		
		public AppStatusCode<Client> Update(ClientRequest clientUpdate, Guid clientid)
		{
			Client item = _clientData.Read<Client>(clientid);
			if(item == null)
			{
				return AppStatusCode<Client>.ItemDontExist;
			}
			item.name = clientUpdate.name;
			item.updatedate = DateTime.Now;
			if(_clientData.Update(item))
			{
				return AppStatusCode<Client>.UpdateItem;
			}
			return AppStatusCode<Client>.ServiceUnavailable;
			
		}
	
	}
}
