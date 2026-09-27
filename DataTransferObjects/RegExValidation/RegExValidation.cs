using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
	internal static class RegExValidation
    {
		internal const string name = @"^([a-zA-Z0-9._-]+\\[a-zA-Z0-9._-]+|[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}|[a-zA-Z0-9._-]+)$";
		internal const string remote = @"^(https?:\/\/)?[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*$|^$";
    }
}
