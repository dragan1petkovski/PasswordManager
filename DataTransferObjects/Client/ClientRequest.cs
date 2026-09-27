
using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.Client
{
    public class ClientRequest
    {
		[RegularExpression(RegExValidation.name)]
		public string name { get; set; }
    }
}
