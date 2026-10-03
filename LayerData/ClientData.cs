using DBLayer;
using DataTransferObjects.User;


namespace LayerData
{
    public class ClientData : Data
    {
		public ClientData(MSSQLContext dbContext, Serilog.ILogger logger, UserSession userSession) : base(dbContext, logger, userSession)
		{
			
		}

	}
}
