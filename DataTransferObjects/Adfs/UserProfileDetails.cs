using DataTransferObjects.User;

namespace DataTransferObjects.Adfs
{
    public class UserProfileDetails
    {
        public AccessTokenResponse token_details { get; set;  }
        public UserSession userSession { get; set; }


        public void SetData(AccessTokenResponse _token, UserSession _userSession)
        {
            token_details = _token;
            userSession.SetBasicInfo(_userSession);
        }
    }
}
