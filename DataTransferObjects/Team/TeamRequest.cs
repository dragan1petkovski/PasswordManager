using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Team
{
    public class TeamRequest
    {
		public Guid clientid { get; set; }

		[RegularExpression(RegExValidation.name)]
		public string name { get; set; }

    }
}
