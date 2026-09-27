using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	static internal class ErroMessages
    {
		static internal string ConnectionError(string errorDetails) => $"Connection to the database failed\n{errorDetails}\n";
		static internal string ItemAlreadyExist(string errorDetails) => $"Item alreay exist\n{errorDetails}\n";

		static internal string InvalidId(string errorDetails) => $"Id value should not be NULL\n{errorDetails}\n";
		static internal string ItemDoNotExist(string errorDetails) => $"Item do not exist\n{errorDetails}\n";
    }
}
