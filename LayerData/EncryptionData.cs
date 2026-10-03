using DBLayer;
using DataTransferObjects.User;

namespace LayerData
{
    public class EncryptionData : Data
    {
        public EncryptionData(MSSQLContext dbContext, Serilog.ILogger logger, UserSession userSession) : base(dbContext, logger, userSession)
        {

        }
    }
}
