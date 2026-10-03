using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using DataTransferObjects.User;
namespace Services.Audit
{
	public class JwtManager
	{

		private JwtPayload GetJWTPayload(string jwt)
		{

			try
			{
				JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
				JwtSecurityToken t = handler.ReadJwtToken(jwt);
				return t.Payload;
			}
			catch
			{
				//In decision making for writing logs here - Malformed jwt token
				throw new Exception("JWT token is mailformed");
			}


		}

		public string GetUPN(string jwt)
		{
			try
			{
				return GetJWTPayload(jwt)["upn"].ToString();
				
			}
			catch (Exception e)
			{
				//In decision making for writing logs here
				throw e;
			}

		}

		public string GetEmail(string jwt)
		{
			try
			{
				return GetJWTPayload(jwt)["email"].ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public (bool success, string roleName) GetRole(string jwt)
		{
			try
			{
				string roleName = GetJWTPayload(jwt)[ClaimTypes.Role].ToString();
				if (roleName != null)
				{
					return (true, roleName);
				}
				return (false, null);
			}
			catch
			{
				return (false, null);
			}

		}

		public UserSession GetUserDetails(string jwt)
		{
			try
			{
				var jwtpayload = GetJWTPayload(jwt);
				return new UserSession
                {
					role = jwtpayload["role"].ToString(),
					upn = jwtpayload["upn"].ToString(),
					givenname = jwtpayload["given_name"].ToString(),
					surname = jwtpayload["family_name"].ToString(),
					email = jwtpayload["email"].ToString(),
					

				};
			}
			catch (Exception e)
			{
				throw new Exception($"Jwt token is not correctly parsed: {e.ToString()}");
			}

		}
	}
}
