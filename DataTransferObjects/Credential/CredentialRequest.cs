using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Credential
{
	public class CredentialRequest
	{
		[Required]
		[RegularExpression(RegExValidation.name)]
		public string username { get; set; }
        [RegularExpression(RegExValidation.remote)]
        public string? remote { get; set; }

        public string? password { get; set; }
		public string? key { get; set; }
		
		[Required]
		public Guid teamid { get; set; }


	}
}
