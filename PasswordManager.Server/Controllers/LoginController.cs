using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;
using DataTransferObjects.Adfs;

namespace PasswordManager.Server.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        public LoginController(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        [HttpGet("[controller]/oauth/callback")]
        public IActionResult AdfsCallback([FromQuery] string code, [FromQuery] string state)
        {
            using HttpClient client = new HttpClient();
            Dictionary<string, string> tokentRequest = new Dictionary<string, string>
            {
                {"client_id", this._configuration.GetSection("oauth2").GetSection("client_id").Value },
                {"code", code},
                {"redirect_uri", this._configuration.GetSection("oauth2").GetSection("redirectUri").Value },
                {"grant_type", this._configuration.GetSection("oauth2").GetSection("grant_type").Value},
                {"client_secret", this._configuration.GetSection("oauth2").GetSection("client_secret").Value },
                {"scope", "openid profile email allatclaims api" }
            };
            string authorization_endpoint = this._configuration.GetSection("oauth2").GetSection("authorization_endpoint").Value;
            HttpContent content = new FormUrlEncodedContent(tokentRequest);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var token = client.PostAsync(authorization_endpoint,content).Result;
            if(token.IsSuccessStatusCode)
            {
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(10),
                    Domain = "cm.test.local",
                    SameSite = SameSiteMode.Strict
                };
                AccessTokenResponse tokenObj = JsonSerializer.Deserialize<AccessTokenResponse>(token.Content.ReadAsStringAsync().Result);
                Guid tokenId = Guid.NewGuid();
                this._cache.Set<AccessTokenResponse>(tokenId, tokenObj, TimeSpan.FromMinutes(10));
                this.Response.Cookies.Append("_id",tokenId.ToString() , cookieOptions);
                return Redirect("/auth");

            }
            
            return NoContent();

        }
    }
}
