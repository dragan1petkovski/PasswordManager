using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvcEncryptionExceptions
{
    public class MasterPasswordException : Exception
    {
        public MasterPasswordException()
        {
        }

        public MasterPasswordException(string message)
            : base(message)
        {
        }
    }
}
