using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel
{
    public class Credential
    {
        [Key] public Guid id { get; set; }
        public string username { get; set; }
        public string? remote { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
		public byte[]? password { get; set; }
        public byte[]? key { get; set; }

        public Team team { get; set; }
        [ForeignKey(nameof(team))]
        public Guid teamid { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


    }
}
