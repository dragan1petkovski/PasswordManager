using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DomainModel
{
    public class User
    {
        public Guid id { get; set; }

        [Key][MaxLength(450)]
        public string userprinciplename { get; set; } //UserPrincipleName
		public string email { get; set; } //mail
		public string username { get; set; } //sAMAccountName
        public string firstname { get; set; } //givenName
        public string lastname { get; set; } //sn


        public string role { get; set; } //Hardwritten\


        public DateTime createdate { get; set; }
		public DateTime updatedate { get; set; }
		
		//public string privatekey { get; set; }
		//public bool MFAenabled { get; set; }

        public ICollection<Team>? teams { get; set; }
		

        [NotMapped]
        public string fullName => firstname + " " + lastname;
    }
}
