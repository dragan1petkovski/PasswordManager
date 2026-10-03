using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Services.Audit
{
	public class JwtWebKeyProcessor
	{
		//TO DO: Make the code more resiliat on ADFS failure
		public static IEnumerable<SecurityKey>  GetADFSKeys(IConfiguration config)
		{
			var oidc = config.GetSection("oauth2");
			using (HttpClient httpClient = new HttpClient())
			{
				HttpResponseMessage jwks_response = httpClient.GetAsync(oidc["jwks_uri"].ToString()).Result;
				var jwks = jwks_response.Content.ReadAsStringAsync().Result;

				var keyObj = JsonWebKeySet.Create(jwks);

				return keyObj.GetSigningKeys().ToList();
			}
		}
    }
}
