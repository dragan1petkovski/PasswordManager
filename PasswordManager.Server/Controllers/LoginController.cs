using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;
using DataTransferObjects.Adfs;
using DataTransferObjects.User;
using Services.Audit;
using DataTransferObjects.User;
using System.Web;

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
        public IActionResult AdfsCallback([FromQuery] string code, [FromQuery] string state, [FromServices] JwtManager jwtManager)
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

                UserSession _userSession = jwtManager.GetUserDetails(tokenObj.access_token);

                UserProfileDetails _userProfileDetails = new UserProfileDetails() { userSession = _userSession, token_details = tokenObj };
                this._cache.Set<UserProfileDetails>(tokenId, _userProfileDetails, TimeSpan.FromMinutes(10));
                this.Response.Cookies.Append("_id",tokenId.ToString() , cookieOptions);

                var temp = this._cache.Get<UserProfileDetails>(tokenId);
                return Redirect("/auth");

            }
            
            return NoContent();

        }

        [HttpGet("logout")]
        public IActionResult AdfsSignout()
        {
            using HttpClient client = new HttpClient();
            Guid cookieId = Guid.Parse(Request.Cookies["_id"]);
            AccessTokenResponse token = _cache.Get<UserProfileDetails>(cookieId).token_details;

            UriBuilder uriBuilder = new UriBuilder(this._configuration.GetSection("oauth2").GetSection("logouturi").Value);
            var queryparams = HttpUtility.ParseQueryString(string.Empty);
            queryparams["id_token_hint"] = token.id_token;
            queryparams["post_logout_redirect_uri"] = this._configuration.GetSection("oauth2").GetSection("logoutredirecuri").Value;
            uriBuilder.Query = queryparams.ToString();
            CookieOptions cookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(10),
                Domain = "cm.test.local",
                SameSite = SameSiteMode.Strict
            };
            
            Response.Cookies.Delete("_id", cookieOptions);
            _cache.Remove(cookieId);
            return Ok(uriBuilder.Uri.ToString());
        }
    }
}
