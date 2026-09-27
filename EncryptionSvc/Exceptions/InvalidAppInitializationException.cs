using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvcEncryptionExceptions
{
    public class InvalidAppInitializationException: Exception
    {
        public InvalidAppInitializationException()
        {
        }

        public InvalidAppInitializationException(string message)
            : base(message)
        {
        }
    }
}
