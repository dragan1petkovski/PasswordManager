using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;
using DataTransferObjects.Adfs;
using DataTransferObjects.User;
using Services.Audit;
using PasswordManager.Server.Utilities;
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
            //If users is already loggedin to stop making unnecessary request and filling cache 
            if (Guid.TryParse(Request.Cookies["_id"], out Guid cookieid))
            {
                UserProfileDetails userDetails = _cache.Get<UserProfileDetails>(cookieid);
                if(userDetails is not null)
                {
                    return Redirect("/auth");
                }
            }


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
                    Domain = "cm.test.local", //This need to change the reflect binding
                    SameSite = SameSiteMode.Strict
                };
                string rawtoken = token.Content.ReadAsStringAsync().Result;
                AccessTokenResponse tokenObj = JsonSerializer.Deserialize<AccessTokenResponse>(rawtoken);
                Guid tokenId = Guid.NewGuid();

                UserSession _userSession = jwtManager.GetUserDetails(tokenObj.access_token);

                this._cache.Set<UserSession>(CacheService.GetUserSessionCacheKey(tokenId),_userSession , TimeSpan.FromMinutes(10));
                this._cache.Set<AccessTokenResponse>(CacheService.GetAdfsCacheKey(tokenId), tokenObj, TimeSpan.FromMinutes(10));

                this.Response.Cookies.Append("_id",tokenId.ToString() , cookieOptions);

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
                Domain = "cm.test.local", //This need to change the reflect binding
                SameSite = SameSiteMode.Strict
            };
            
            Response.Cookies.Delete("_id", cookieOptions);

            _cache.Remove(CacheService.GetUserSessionCacheKey(cookieId));
            _cache.Remove(CacheService.GetAdfsCacheKey(cookieId));
            _cache.Remove(CacheService.GetPasswordCacheKey(cookieId));

            return Ok(uriBuilder.Uri.ToString());
        }
    }
}
