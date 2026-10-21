namespace DataTransferObjects.User
{
    public class UserSession
    {
        public string givenname { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        public string upn { get; set; } // this is Windows UserPrinciple Name
        public string action {
            get;
            set
            {
                field = value switch
                {
                    "PUT" => "UPDATE",
                    "POST" => "CREATE",
                    "DELETE" => "DELETE",
                    _ => "UNKNOWN"
                };
            }
        }

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
