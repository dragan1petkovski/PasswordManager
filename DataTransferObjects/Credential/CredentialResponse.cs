using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.Credential
{
	public class CredentialResponse
	{
		public Guid id { get; set; }

        [RegularExpression(RegExValidation.name)]
        public string username { get; set; }

        [RegularExpression(RegExValidation.remote)]
        public string? remote { get; set; }

        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
        public bool password { get; set; }
        public bool key { get; set; }
		public Guid teamid { get; set; }
		public string teamname { get; set; }

	}
}
