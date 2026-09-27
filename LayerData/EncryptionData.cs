using DataLayer;
using DBLayer;
using Services.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerData
{
    public class EncryptionData : Data
    {
        public EncryptionData(MSSQLContext dbContext, Serilog.ILogger logger, UserSession userSession) : base(dbContext, logger, userSession)
        {

        }
    }
}
