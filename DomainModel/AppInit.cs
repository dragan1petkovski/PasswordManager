using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class AppInit
    {
        [Key] public Guid id { get; set; }
        public byte[] mk { get; set; }
        public DateTime initializationTime { get; set; } 
        public bool initialization { get; set;  }

        public byte[] ValidationMessage { get; set; }

        

    }
}
