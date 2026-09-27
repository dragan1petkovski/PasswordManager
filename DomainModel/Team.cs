using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel
{
	public class Team
	{
		[Key] public Guid id { get; set; }

		public string name { get; set; }

		public DateTime createdate { get; set; }
		public DateTime updatedate { get; set; }

		public ICollection<Certificate>? certificates { get; set; }
		public ICollection<Credential>? credentials { get; set; }

		public Client client { get; set; }

		[ForeignKey(nameof(client))]
		public Guid clientid { get; set; }

		public ICollection<User>? users { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
