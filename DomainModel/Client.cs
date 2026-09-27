using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DomainModel
{
	public class Client
    {
        [Key]
        public Guid id { get; set; }
        
        [MaxLength(450)]
        public string name { get; set; }

        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }

        public ICollection<Team> teams { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
