using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Credential
{
    public class GiveCredentialRequest
    {
        public CredentialRequest credential {  get; set; }
        public List<Guid>? teams { get; set; }

        public List<Guid>? users { get; set; }
    }
}
