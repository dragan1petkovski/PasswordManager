using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.Credential
{
    public class CreadentialUpdateRequest
    {
		[Required]
		public Guid id { get; set; }

		[Required]
		[RegularExpression(RegExValidation.name)]
		public string username { get; set; }

        public string? password { get; set; }
		public string? email { get; set; }

        [RegularExpression(RegExValidation.remote)]
        public string? remote { get; set; }

	}
}
