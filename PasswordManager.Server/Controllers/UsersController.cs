using DataTransferObjects.Adfs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using PasswordManager.Server.StaticObjects;

namespace PasswordManager.Server.Controllers
{
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        public UsersController(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        [HttpGet("/[controller]/me")]
        [Authorize(Roles = $"{AdfsRoles.userrole}, {AdfsRoles.adminrole}")]
        public IActionResult Me()
        {
            if(Guid.TryParse(this.Request.Cookies["_id"],out Guid tokenId))
            {
                AccessTokenResponse adfsResponse = _cache.Get<AccessTokenResponse>(tokenId);
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken access_token = handler.ReadJwtToken(adfsResponse.access_token);
                UserProfile profile = new UserProfile()
                {
                    display_name = access_token.Payload["unique_name"].ToString(),
                    email = access_token.Payload["email"].ToString(),
                    surname = access_token.Payload["family_name"].ToString(),
                    role = access_token.Payload["role"].ToString(),
                    given_name = access_token.Payload["given_name"].ToString()

                };
                return Ok(profile);
            }
            return NotFound();
        }
    }
}
