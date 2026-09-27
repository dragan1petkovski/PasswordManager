using DomainModel;

namespace Services.Audit
{
    public class UserSession
    {
        public string givenname { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        public string upn { get; set; } // this is Windows UserPrinciple Name
		public string? httpMethod { get; set; }
		public DateTime? HttpRequestTime { get; set; }


        public void SetBasicInfo(UserSession temp)
        {
            this.givenname = temp.givenname;
            this.surname = temp.surname;
            this.email = temp.email;
            this.role = temp.role;
            this.upn = temp.upn;
        }


    }
}
